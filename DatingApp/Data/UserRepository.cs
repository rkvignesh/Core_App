﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTO;
using DatingApp.Entities;
using DatingApp.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper) 
        {
            _context = dataContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDTO>> GetMemberAsync()
        {
            return await _context.Users
               .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }

        public async Task<MemberDTO> GetMemberByUserNameAsync(string username)
        {
            return await _context.Users
                .Where(user => user.UserName == username)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.Users
                .Include(p=>p.Photos)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
