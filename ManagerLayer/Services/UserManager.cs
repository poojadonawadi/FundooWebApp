using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository user;
        private readonly FundooDBContext context;

        public UserManager(IUserRepository user, FundooDBContext context)
        {
            this.user = user;
            this.context = context;
        }

        public Users Registration(RegisterModel model)
        {
            return user.Registration(model);
        }

        public bool MailExists(string email)
        {
            var checkMailExist = this.context.Users.FirstOrDefault(x => x.Email == email);
            if (checkMailExist != null)

            {
                return true;
            }
            return false;
        }
        public string Login(LoginModel login)
        {
            return user.Login(login);
        }



        public ForgotPasswordModel ForgotPasswordModel(string Email)
        {
            return user.ForgotPasswordModel(Email);
        }


        public bool ResetPassword(string email, string password, string confirmPassword)
        {
            return user.ResetPassword(email, password, confirmPassword);
        }
    }

}