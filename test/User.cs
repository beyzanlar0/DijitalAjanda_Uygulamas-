using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class User
    {

        public int UserId { get; set; }
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        //public void SetPassword(string password)
        //{
        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        //}


        //public bool VerifyPassword(string password)
        //{
        //    return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        //}
    }
}
