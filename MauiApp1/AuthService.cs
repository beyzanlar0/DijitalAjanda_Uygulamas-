using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class AuthService
    {
        private readonly DataBase _context;

        public AuthService()
        {
            _context = new DataBase();
        }

        public bool OnRegisterClicked(string namesurname, string username,string email, string phone, string password)
        {
            _context.Database.EnsureCreated();
            if (_context.Users.Any(u => u.Email == email))
            {
                return false; // Kullanıcı zaten var
            }

            var newUser = new User
            {
                NameSurname = namesurname,
                UserName = username,
                Email = email,
                MobileNumber = phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) // Şifreyi hashle
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return true;
        }

        public bool dogrula(string email,string sifre)
        {
            var kullanici = _context.Users.FirstOrDefault(k=>k.Email==email);
            if (kullanici != null && kullanici.VerifyPassword(sifre)) return true;
            else return false;
        }

        
    }

}
