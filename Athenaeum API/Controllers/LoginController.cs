using Microsoft.AspNetCore.Mvc;
using Athenaeum_API.Models;
using Athenaeum_API.Data;

namespace Athenaeum_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly APIContext context;

        public LoginController(APIContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public JsonResult CreateEdit(User user)
        {

            var FoundUser = context.Users.Any(x => x.USERNAME == user.USERNAME && x.PASSWORD == user.PASSWORD);

            if (FoundUser != false)
            {
                return new JsonResult(new { message = "User already Exists" }) { StatusCode = 500 };
            }

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
        [HttpGet("/LoginRequest")]
        public JsonResult LoginRequest([FromQuery]string username, string password)
        {
            
            var UserExists = context.Users.Any(x => x.USERNAME == username && x.PASSWORD == password);

            if (UserExists == true)
            {
                var FoundUser = context.Users.First(x => x.USERNAME == username && x.PASSWORD == password);

                FoundUser.LAST_LOGIN = DateTime.Now;

                context.SaveChanges();

                return new JsonResult(Ok(true));
            }
            else
            {

                return new JsonResult(Ok(false));

            }
        }


    }
}
