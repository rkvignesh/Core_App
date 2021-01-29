using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatingApp.Interface;

namespace DatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;

        public AccountController(DataContext Context, ITokenService tokenService)
        {
            _tokenService = tokenService;

            _context = Context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            try
            {
                if (await UserExists(registerDTO.username))
                {
                    return BadRequest("UserName already exists");
                }

                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = registerDTO.username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                    PasswordSalt = hmac.Key
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return new UserDTO
                {
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
            catch (Exception e)
            {
                return BadRequest("Exception: " + e.Message);
            }
        }

        private async Task<bool> UserExists(string username)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> login(LoginDTO loginDTO)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.username);

                if (user == null)
                {
                    return Unauthorized("invalid username");
                }

                using var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHASH = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));

                for (int i = 0; i < computedHASH.Length; i++)
                {
                    if (user.PasswordHash[i] != computedHASH[i])
                        return Unauthorized("invalid password");
                }

                return new UserDTO
                {
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
            catch (Exception e)
            {
                return BadRequest("Exception: " + e.Message);
            }
        }
    }
}
