using MainMailApiMultiSwagger.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
        [Route("CheckUserRole")]
        [Authorize(Roles = "Adminstrator, Adminhelper")]
        public async Task<IActionResult> CheckUser(string name)
        {
            var token = HttpContext.Session.GetString("JWToken");
            try
            {            
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://localhost:7175/Restricted/GetUserRole?name={name}");

                request.Headers.TryAddWithoutValidation("accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                request.Content = new StringContent("");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                var response = await httpClient.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return Ok(apiResponse);
                }
                return StatusCode(500);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("ChangeUsersRole")]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> ChangeRole(string name)
        {
            var token = HttpContext.Session.GetString("JWToken");
            try
            {
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("PUT"), $"https://localhost:7175/Restricted/ChangeUserRole?name={name}");
                request.Headers.TryAddWithoutValidation("accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                var response = await httpClient.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return Ok(apiResponse);
                }
                return StatusCode(500);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "Adminstrator, Adminhelper")]
        public async Task<IActionResult> DeleteUser(string name)
        {
            var token = HttpContext.Session.GetString("JWToken");
            StringBuilder sb = new StringBuilder();
            try
            {
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"https://localhost:7175/Restricted/DeleteUser?name={name}");
                request.Headers.TryAddWithoutValidation("accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                var response = await httpClient.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //string apiResponse = await response.Content.ReadAsStringAsync();
                    //*************
                    using var httpClient2 = new HttpClient();
                    using var response2 = await httpClient.GetAsync($"https://localhost:7126/Message/GetAllMessages?userName={name}");
                    if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse2 = await response2.Content.ReadAsStringAsync();
                        var postList2 = JsonConvert.DeserializeObject<List<MessageDto>>(apiResponse2);
                        //**
                        //**
                        foreach(var post in postList2)
                        {
                            using var httpClient3 = new HttpClient();
                            using var request3 = new HttpRequestMessage(new HttpMethod("DELETE"), $"https://localhost:7126/Message/DeleteMessage?Id={post.Id}");
                            request3.Headers.TryAddWithoutValidation("accept", "*/*");
                            var response3 = await httpClient.SendAsync(request3);
                            if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                //string apiResponse3 = await response.Content.ReadAsStringAsync();
                                //sb.Append($"message id:{post.Id} delited\n");
                                sb.Append($"{await response.Content.ReadAsStringAsync()} \n");
                            }
                            else 
                                sb.Append($"{await response.Content.ReadAsStringAsync()} \n");
                        }
                        //**
                        //**
                    }
                    //****************
                    return Ok($"user {name} successful deleted\n" + sb);
                    //return Ok($"{apiResponse}\n + {sb}");
                }
                return StatusCode(500);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}


