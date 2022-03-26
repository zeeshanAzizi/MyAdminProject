using MyAdminProject.Models;
using MyAdminProject.Repository.Interface;
using MyAdminProject.Utils.Enums;
using MyAdminProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyAdminProject.Repository.Services
{
    public class AccountService : IUsers
    {
        private MSDbContext dbContext;
        public AccountService()
        {
            dbContext = new MSDbContext();
        }
        public SignInEnum SignIn(SignInModel model)
        {
            var user = dbContext.Tbl_Users.SingleOrDefault(e => e.Email == model.Email && e.Password == model.Password);
            if (user != null)
            {
                if (user.IsVerified)
                {
                    if (user.IsActive)
                    {
                        return SignInEnum.Success;
                    }
                    else
                    {
                        return SignInEnum.InActive;
                    }
                }
                else
                {
                    return SignInEnum.NotVerified;
                }
            }
            else
            {
                return SignInEnum.WrongCredentials;
            }
        }

        public SignUpEnums SignUp(SignUpModel model)
        {
            if (dbContext.Tbl_Users.Any(e => e.Email == model.Email))
            {
                return SignUpEnums.EmailExist;
            }
            else
            {
                var user = new Tbl_Users()
                {
                    Fname = model.Fname,
                    Lname = model.Lname,
                    Password = model.Password,
                    Gender = model.Gender,
                    Email = model.Email
                };
                dbContext.Tbl_Users.Add(user);
                string Otp = GenerateOTP();
                SendMail(model.Email, Otp);
                var VAccount = new VerifyAccount()
                {
                    Otp = Otp,
                    Userid = model.Email,
                    SendTime = DateTime.Now
                };
                dbContext.VerifyAccounts.Add(VAccount);
                dbContext.SaveChanges();
                return SignUpEnums.Success;
            }
            return SignUpEnums.Failure;

        
    }
        private void SendMail(string to, string Otp)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("azizipower786@gmail.com");
            mail.Subject = "verify your Account";
            string Body = $"Your OTP is <b> {Otp}</b> <br/>thanks for choosing us.";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("azizipower786@gmail.com", "your password");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        private string GenerateOTP()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, 8).Select(x => chars[random.Next(chars.Length)]);
            var r = string.Join("", list);

            return r;
        }
    }
}
