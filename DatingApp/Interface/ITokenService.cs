using DatingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
