using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLayer;
using CommonLayer.Models;
using ManagerLayer;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;


namespace FundooNotesApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;
        private readonly IUserManager manager;
        private readonly IBus bus;

        public UsersController(IUserManager manager, FundooDBContext context, IConfiguration configuration, IBus bus)
        {
            this.context = context;
            this.configuration = configuration;
            this.manager = manager;
            this.bus = bus;
        }
        [HttpPost]
        [Route("Reg")]
        public IActionResult Register(RegisterModel model)
        {
            var checkEmail = manager.MailExists(model.Email);
            if (checkEmail)
            {
                return BadRequest(new ResponseModel<bool> { Success = true, Message = "email already exists" });
            }
            else
            {
                var result = manager.Registration(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<Users> { Success = true, Message = "register successful", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<Users> { Success = false, Message = "registration failed" });
                }
            }


        }
        [HttpPost]
        [Route("Login")]

        public IActionResult UserLogin(LoginModel model)
        {
            var response = manager.Login(model);
            if (response != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "login successful", Data = response.ToString() });
            }
            return BadRequest(new ResponseModel<string> { Success = false, Message = "login failed" });
        }


        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                if (manager.MailExists(email))
                {

                    ForgotPasswordModel forgotPasswordmodel = manager.ForgotPasswordModel(email);
                    Send send = new Send();
                    send.SendMail(forgotPasswordmodel.Email, forgotPasswordmodel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgotPasswordmodel);
                    return Ok(new ResponseModel<string> { Success = true, Message = "mail sent successfully", Data = endPoint.ToString() });
                }
                return BadRequest(new ResponseModel<string> { Success = true, Message="Email provided is not registered" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Reset")]

        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                var Email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

                var result = manager.ResetPassword(Email, password, confirmPassword);

                if (result)
                {
                    return Ok(new { Success = true, Message = "Reset Password Successful" });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = "Reset Password not Sent" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}