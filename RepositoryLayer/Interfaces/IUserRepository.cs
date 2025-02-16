using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        public Users Registration(RegisterModel model);

        public string Login(LoginModel login);

        public ForgotPasswordModel ForgotPasswordModel(string Email);

        public bool ResetPassword(string email, string password, string confirmPassword);
    }
}