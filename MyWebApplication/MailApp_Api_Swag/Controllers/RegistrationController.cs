using MainMailApiMultiSwagger.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MainMailApiMultiSwagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Tags("Registration Service Endpoints")]
    [ApiExplorerSettings(GroupName = "registration")]
    public class RegistrationController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost]
        [Route("AdminRegistration")]
        public async Task<IActionResult> AddAdmin(string adminName, string adminPassword)
        {
            try
            {
                LoginModel loginModel = new LoginModel();
                loginModel.Name = adminName;
                loginModel.Password = adminPassword;

                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:7175/Login/AddAdmin");
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
                    return Ok("Administrator successfully registered!");
                }
                //return Ok(token);
                return StatusCode(401, "Administrator is already registered!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }  
        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "Adminstrator, Adminhelper")]
        public async Task<IActionResult> AddUser(string userName, string userPassword)
        {
            var token = HttpContext.Session.GetString("JWToken");
            try
            {
                LoginModel loginModel = new LoginModel();
                loginModel.Name = userName;
                loginModel.Password = userPassword;

                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://localhost:7175/Login/AddUser");

                request.Headers.TryAddWithoutValidation("accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(loginModel));
                request.Content = content;
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok("User successfully registered!");
                }
                return StatusCode(401, "User is already registered!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("ShowAllUsers")]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> GetAllUsers()
        {
            var token = HttpContext.Session.GetString("JWToken");
            try
            {
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("GET"), "https://localhost:7175/Restricted/GetAllUsers");

                request.Headers.TryAddWithoutValidation("accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var usersList = JsonConvert.DeserializeObject<List<UserModel>>(apiResponse);
                    return Ok(usersList);
                }
                return StatusCode(500);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("CheckUsersRole")]
        [Authorize(Roles = "Adminstrator, Adminhelper")]
        public ActionResult<bool> CheckUser(string name)
        {

            //var res = _userRepository.UserExists(name);
            //if (res == false)
            //    return StatusCode(500);
            return Ok();
        }

        [HttpPost]
        [Route("ChangeUsersRole")]
        [Authorize(Roles = "Adminstrator")]
        public ActionResult<bool> ChangeRole(string name)
        {

            //var res = _userRepository.UserExists(name);
            //if (res == false)
            //    return StatusCode(500);
            return Ok();
        }
        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "Adminstrator, Adminhelper")]
        public async Task<IActionResult> DeleteUser(string name)
        {
            //_userRepository.DeleteUser(name);
            return Ok();
        }
    }
}
