using MainMailApiMultiSwagger.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MainMailApiMultiSwagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Tags("LogInOut Service Endpoints")]
    [ApiExplorerSettings(GroupName = "logInOut")]
    public class LogInOutController : ControllerBase
    {
        private string ActiveUserName()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var DecodedJWT = handler.ReadJwtToken(token);
            var activeUserName = DecodedJWT.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return activeUserName;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string userName, string userPassword)
        {
            try
            {
                LoginModel loginModel = new LoginModel();
                loginModel.Name = userName;
                loginModel.Password = userPassword;

                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:7175/Login");
                request.Headers.TryAddWithoutValidation("accept", "*/*");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(loginModel));
                request.Content = content;
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    //if (token != null)
                    //{
                    HttpContext.Session.SetString("JWToken", token);
                    //return Ok("Authenticate successful done.");
                    return Ok(token);
                }

                return StatusCode(401, "Authenticate NOT done.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet]
        [Route("ShowActiveUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowActiveUser()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
            {
                var token = HttpContext.Session.GetString("JWToken");
                var activeUserName = ActiveUserName();
                return Ok(activeUserName);
            }
            return Ok("No active user. Please, log in!");
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
