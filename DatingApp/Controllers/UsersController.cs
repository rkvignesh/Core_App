using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext Context)
        {
            _context = Context;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            try
            { 
                return await _context.Users.ToListAsync();
            }
            catch (Exception e)
            { 
                Console.WriteLine(e);

                return null;
            }
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
