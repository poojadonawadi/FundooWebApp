using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CommonLayer;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
//using RepositoryLayer.Migrations;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public UserRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public Users Registration(RegisterModel model)
        {
            Users users = new Users();
            users.FirstName = model.FirstName;
            users.LastName = model.LastName;
            users.DOB = model.DOB;
            users.Gender = model.Gender;
            users.Email = model.Email;
            users.Password = EncodePassword(model.Password);
            context.Users.Add(users);
            context.SaveChanges();
            return users;

        }

        public static string EncodePassword(string password)
        {
            try
            {
                byte[] enData = new byte[password.Length];
                enData = System.Text.Encoding.UTF8.GetBytes(password);
                string encodeData = Convert.ToBase64String(enData);
                return encodeData;
            }
            catch (Exception ex)
            {
                throw new Exception("error in base64Encode" + ex.Message);
            }
        }

        public string Login(LoginModel login)
        {
            var CheckEmail = context.Users.FirstOrDefault(a => a.Email == login.Email && a.Password == EncodePassword(login.Password));
            if (CheckEmail != null)
            {
                string token = GenerateToken(CheckEmail.UserId, CheckEmail.Email);
                return token;

            }
            return null;
        }

        private string GenerateToken(int userId, string EmailId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("UserId",userId.ToString()),
                new Claim("Email",EmailId)
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public ForgotPasswordModel ForgotPasswordModel(string Email)
        {
            Users user = context.Users.ToList().Find(a => a.Email == Email);
            if (user != null)
            {
                ForgotPasswordModel forgotPasswordmodel = new ForgotPasswordModel();
                forgotPasswordmodel.UserId = user.UserId;
                forgotPasswordmodel.Email = user.Email;
                forgotPasswordmodel.Token = GenerateToken(user.UserId, user.Email);
                return forgotPasswordmodel;

            }
            else
            {
                throw new Exception("user doesnot exists for this email");
            }
        }

        public bool ResetPassword(string email, string password, string confirmPassword)
        {
            try
            {
                if (password.Equals(confirmPassword))
                {
                    var emailCheck = context.Users.FirstOrDefault(x => x.Email == email);
                    emailCheck.Password = password;

                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}