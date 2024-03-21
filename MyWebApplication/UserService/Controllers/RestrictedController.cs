using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.AuthorizationModel;
using UserService.Repository;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestrictedController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public RestrictedController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = "Adminstrator")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }

        [HttpGet]
        [Route("Users")]
        [Authorize(Roles = "Adminstrator, User")]
        public IActionResult UserEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Adminstrator")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _userRepository.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpPost]
        [Route("CheckUser")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public ActionResult<bool> CheckUser(string name)
        {
            var res = _userRepository.UserExists(name);
            if (res == false)
                return StatusCode(500);
            return Ok();
        }

        [HttpPost]
        [Route("GetUserRole")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public ActionResult GetUserRole(string name)
        {
            var res = _userRepository.GetUserRole(name);
            return Ok(res);
        }

        [HttpPut]
        [Route("ChangeUserRole")]
        [Authorize(Roles = "Adminstrator")]
        public IActionResult ChangeUserRole(string name)
        {
            var res = _userRepository.ChangeUserRole(name);
            return Ok(res);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "Adminstrator, Adminhelper")]
        public IActionResult DeleteUser(string name)
        {
            _userRepository.DeleteUser(name);
            return Ok();
        }

        private UserDto GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserDto
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = (UserRole)Enum.Parse(typeof(UserRole),
                        userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }
    }
}