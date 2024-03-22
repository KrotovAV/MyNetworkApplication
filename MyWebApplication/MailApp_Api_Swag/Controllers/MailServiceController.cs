using MainMailApiMultiSwagger.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace MainMailApiMultiSwagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Tags("Mail Service Endpoints")]
    [ApiExplorerSettings(GroupName = "mail")]
    public class MailServiceController : ControllerBase
    {
        //private readonly ILogger<MailServiceController> _logger;
        //private readonly IConfiguration _config;

        //public MailServiceController(ILogger<MailServiceController> logger, IConfiguration config)
        //{ 
        //    _logger = logger;
        //    _config = config;
        //}

        private string ActiveUserName()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var DecodedJWT = handler.ReadJwtToken(token);
            var activeUserName = DecodedJWT.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return activeUserName;
        }

        private async Task<bool> CheckUserExist(string userNameForCheck)
        {
            var token = HttpContext.Session.GetString("JWToken");
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://localhost:7175/Restricted/CheckUser?name={userNameForCheck}");
            request.Headers.TryAddWithoutValidation("accept", "text/plain");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
            request.Content = new StringContent("");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            var response2 = await httpClient.SendAsync(request);
            if (response2.StatusCode == System.Net.HttpStatusCode.OK) 
                return true;
            return false;
        }

        [HttpGet]
        [Route("GetAllMessages")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public async Task<IActionResult> GetAllMessages() 
        {
            string receiverName = ActiveUserName();
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync($"https://localhost:7126/Message/GetAllMessages?userName={receiverName}");
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
                string apiResponse = await response.Content.ReadAsStringAsync();
                var postList = JsonConvert.DeserializeObject<List<MessageDto>>(apiResponse);
                return Ok(postList);
            //}
            //return StatusCode(500);
        }

        [HttpGet]
        [Route("GetUnreceivedMessages")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public async Task<IActionResult> GetUnreceivedMessages()
        {
            string receiverName = ActiveUserName();
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync($"https://localhost:7126/Message/GetUnreceivedMessages?userName={receiverName}");
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
                string apiResponse = await response.Content.ReadAsStringAsync();
                var postList = JsonConvert.DeserializeObject<List<MessageDto>>(apiResponse);
                return Ok(postList);
            //}
            //return StatusCode(500);
        }

        [HttpGet]
        [Route("CheckUser")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public async Task<IActionResult> CheckUser(string receiverName)
        {
            bool receverExist = await CheckUserExist(receiverName);
            if (receverExist == true)
                return Ok($"User {receiverName} exist");
            return StatusCode(500);
        }

        [HttpPost]
        [Route("SendMessage")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public async Task<IActionResult> SendMessage(string receiverName, string text)
        {
            bool receverExist = await CheckUserExist(receiverName);
            if (receverExist == true)
            {
                MessageModel messageModel = new MessageModel();
                messageModel.SenderName = ActiveUserName();
                messageModel.Text = text;
                messageModel.ReceiverName = receiverName;

                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:7126/Message/SendMessage");
                request.Headers.TryAddWithoutValidation("accept", "*/*");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(messageModel));
                request.Content = content;
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await httpClient.SendAsync(request);
                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                var messageId = await response.Content.ReadAsStringAsync();

                return Ok(messageId);
                //}
                //else
                //    return StatusCode(500);
            }
            return NotFound("Receiver not found in base.");
        }

        [HttpDelete]
        [Route("DeleteMessage")]
        [Authorize(Roles = "Adminstrator, User, Adminhelper")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"https://localhost:7126/Message/DeleteMessage?Id={id}");
            request.Headers.TryAddWithoutValidation("accept", "*/*");

            var response = await httpClient.SendAsync(request);
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
                string apiResponse = await response.Content.ReadAsStringAsync();
                return Ok(apiResponse);
            //}
            //return StatusCode(500);
        }
    }
}