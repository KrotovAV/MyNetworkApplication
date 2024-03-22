using MailApp_API_Gateway.Dto;
using MailApp_API_Gateway.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MailApp_API_Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMailRepository _mailRepository;
        //private UserContext _userRepository;

        public MailController(IConfiguration configuration, IMailRepository mailRepository)
        {
            _configuration = configuration;
            _mailRepository = mailRepository;
        }

        //[HttpGet]
        //[Route("GetMessages")]
        //public IActionResult GetAllMessages(string userName)
        //{
        //    var messages = _mailRepository.GetAllMessages(userName);
        //    return Ok(messages);
        //}

        //$"https://localhost:7126/Message/GetMessages?userName={userName}"

        //https://localhost:7126/swagger/v1/swagger.json
        [HttpGet("posts")]
        public async Task<IActionResult> GetAllMessages(string receiverName)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync($"https://localhost:7126/Message/GetMessages?userName={receiverName}");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var postList = JsonConvert.DeserializeObject<List<MessageDto>>(apiResponse);

            return Ok(postList);
        }

        //[HttpGet("posts/{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    using var httpClient = new HttpClient();
        //    using var response = await httpClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}");
        //    string apiResponse = await response.Content.ReadAsStringAsync();
        //    var post = JsonConvert.DeserializeObject<MessageDto>(apiResponse);

        //    return Ok(post);
        //}
    }
}
