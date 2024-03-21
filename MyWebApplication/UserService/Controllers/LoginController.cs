using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Middleware;
using UserService.AuthorizationModel;
using DataBaseUsers.BD;

namespace UserService.Controllers
{
    public static class RSATools
    {
        public static RSA GetPrivateKey()
        {
            var f = File.ReadAllText("rsa/private_key.pem");

            var rsa = RSA.Create();
            rsa.ImportFromPem(f);
            return rsa;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _config;
        //private readonly IUserAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;


        //public LoginController(IConfiguration config, IUserAuthenticationService service, IUserRepository userRepository)
        public LoginController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            //_authenticationService = service;
            _userRepository = userRepository;
        }

        private static UserRole RoleIDToRole(RoleId roleId)
        {
            if (roleId == RoleId.Admin) return UserRole.Adminstrator;

            return UserRole.User;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginModel userLogin)
        {
            try
            {
                var roleId = _userRepository.UserCheck(userLogin.Name, userLogin.Password);

                var user = new UserDto { Username = userLogin.Name, Role = RoleIDToRole(roleId) };

                var token = GenerateToken(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("AddAdmin")]
        public ActionResult AddAdmin([FromBody] LoginModel userLogin)
        {
            try
            {
                _userRepository.UserAdd(userLogin.Name, userLogin.Password, RoleId.Admin);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "Adminstrator")]
        public ActionResult AddUser([FromBody] LoginModel userLogin)
        {
            try
            {
                _userRepository.UserAdd(userLogin.Name, userLogin.Password, RoleId.User);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("CheckUser")]
        [Authorize(Roles = "Adminstrator, User")]
        public ActionResult<bool> CheckUser([FromBody] string name)
        {

            var res = _userRepository.UserExists(name);
            if(res == false)
                return StatusCode(500);
            return Ok();
        }

        private string GenerateToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //var securityKey = new RsaSecurityKey(RSATools.GetPrivateKey());

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256Signature);
            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())};

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

//namespace UserApi.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class UserController : ControllerBase
//    {

//        private readonly IUserService _userService;
//        private readonly Account _account;
//        private readonly IConfiguration _configuration;


//        public UserController(IUserService userService, Account account, IConfiguration configuration)
//        {
//            _account = account;
//            _userService = userService;
//            _configuration = configuration;
//        }

//        [AllowAnonymous]
//        [HttpPost("login")]
//        public ActionResult Login([Description("Аутентификация пользователя"), FromBody] LoginModel model)
//        {
//            if (!Helper.CheckEmail(model.Email))
//                return BadRequest($"Email:'{model.Email}' - Invalid Format");

//            if (_account.GetAccessToken() != null)
//                return BadRequest("Вы уже вошли в систему");
//            var response = _userService.Authentificate(model);
//            if (!response.IsSuccess)
//                return NotFound(response.Errors.FirstOrDefault()?.Message);

//            _account.Login(response.Users[0]);
//            _account.RefreshToken(GenerateToken(_account));

//            return Ok(_account.GetAccessToken());
//        }

//        [Authorize(Roles = "Administrator")]
//        [HttpPost("add")]
//        public ActionResult AddUser(RegistrationModel model)
//        {
//            if (!Helper.CheckEmail(model.Email))
//                return BadRequest($"Email:'{model.Email}' - Invalid Format");
//            if (!Helper.CheckPassword(model.Password))
//                return BadRequest($"Password:'{model.Password}' - Invalid Format");

//            var response = _userService.AddUser(model);
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok(response.UserId);
//        }

//        [AllowAnonymous]
//        [HttpPost("add_admin")]
//        public ActionResult AddAdmin(RegistrationModel model)
//        {
//            if (!Helper.CheckEmail(model.Email))
//                return BadRequest($"Email:'{model.Email}' - Invalid Format");
//            if (!Helper.CheckPassword(model.Password))
//                return BadRequest($"Password:'{model.Password}' - Invalid Format");

//            var response = _userService.AddAdmin(model);
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok(response.UserId);
//        }

//        [Authorize(Roles = "Administrator")]
//        [HttpGet("many")]
//        public ActionResult GetUsers()
//        {
//            var response = _userService.GetUsers();
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok(response.Users);
//        }

//        [Authorize(Roles = "Administrator")]
//        [HttpPost("one")]
//        public ActionResult GetUser(Guid? userId, string? email)
//        {
//            var response = _userService.GetUser(userId, email);
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok(response.Users);
//        }

//        [Authorize(Roles = "Administrator")]
//        [HttpDelete("delete")]
//        public ActionResult DeleteUser(Guid? userId, string? email)
//        {
//            var response = _userService.DeleteUser(userId, email);
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok();
//        }

//        [HttpPost("logout")]
//        public ActionResult LogOut()
//        {
//            _account.Logout();
//            return Ok();
//        }

//        private string GenerateToken(Account model)
//        {
//            var key = new RsaSecurityKey(RSAService.GetPrivateKey());
//            var credential = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Email, model.Email),
//                new Claim(ClaimTypes.Role, model.Role.ToString()),
//                new Claim("UserId", model.Id.ToString())
//            };
//            var token = new JwtSecurityToken(
//                _configuration["Jwt:Issuer"],
//                _configuration["Jwt:Audience"],
//                claims,
//                expires: DateTime.Now.AddMinutes(60),
//                signingCredentials: credential);
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
