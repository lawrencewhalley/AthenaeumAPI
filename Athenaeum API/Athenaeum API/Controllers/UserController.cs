using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athenaeum_API.Models;
using Athenaeum_API.Data;

namespace Athenaeum_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly APIContext context;

        public UserController(APIContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public JsonResult CreateEdit(User user)
        {


            context.Users.Add(user);

            context.SaveChanges();

            return new JsonResult(Ok(User));

        }
        [HttpGet("/GetAll")]
        public JsonResult GetAll()
        {
            var result = context.Users.ToList();

            return new JsonResult(Ok(result));
        }


    }
}
