using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext Context)
        {
            _context = Context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string username, string password) 
        {
            try
            {
                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    PasswordSalt = hmac.Key
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
