using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private ApiDbContext _db;
        public UsersController()
        {
            _db = new ApiDbContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(_db.Users);
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            var users = _db.Users;

            // Validate uniqueness of submitted username
            if (users.FirstOrDefault(u => u.Username == user.Username) != null)
                return HttpBadRequest(new { error = "Username already in use" });

            // Auto increment Id
            if (users.Count == 0)
                user.Id = 1;
            else
                user.Id = users.Last().Id + 1;

            _db.Users.Add(user);
            _db.SaveChanges();

            return new ObjectResult(_db.Users);
        }
    }
}
