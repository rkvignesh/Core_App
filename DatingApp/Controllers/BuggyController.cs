using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    public class BuggyController:BaseApiController
    {
        private readonly DataContext _dataContext;
        public BuggyController(DataContext dataContext) {
            _dataContext = dataContext;
        }

        [HttpGet("not-found")]
        public ActionResult<string> getNotFound()
        {
            var thing = _dataContext.Users.Find(-1);

            //if (thing == null) return NotFound();

            return thing.ToString();
        }

    }
}
