//using DataBaseUsers;
//using DataBaseUsers.Repository;
using DataBaseMessage;
using MessageService.Model;
using MessageService.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageRepository _messageRepository;
        //private UserContext _userRepository;

        public MessageController(IConfiguration configuration, IMessageRepository messageRepository)
        {
            _configuration = configuration;
            _messageRepository = messageRepository;
        }

        [HttpGet]
        [Route("GetAllMessages")]
        //[Authorize(Roles = "Adminstrator, User")]
        public IActionResult GetAllMessages(string userName)
        {
            var messages = _messageRepository.GetAllMessages(userName);
            return Ok(messages);
        }

        [HttpGet]
        [Route("GetUnreceivedMessages")]
        //[Authorize(Roles = "Adminstrator, User")]
        public IActionResult GetUnreceivedMessages(string userName)
        {
            var messages = _messageRepository.GetUnreceivedMessages(userName);
            return Ok(messages);
        }


        [HttpPost]
        [Route("SendMessage")]
        //[Authorize(Roles = "Adminstrator, User")]
        public IActionResult SendMessage([FromBody] MessageModel message)
        {

            //var sender = 1;
            //var recever = 1;
            //if (sender != null && recever != null)
            //{
                var messageId = _messageRepository.SendMessage(
                    message.Text,
                    message.SenderName,
                    message.ReceiverName
                    
                );
                return Ok(messageId);
            //}

            //return NotFound("Sender or Receiver not fount in base");
        }

        [HttpDelete]
        [Route("DeleteMessage")]
        public IActionResult DeleteMessage(Guid Id)
        {
            bool result = _messageRepository.DeleteMessage(Id);
            if(result == true) 
                return Ok("Message delete");
            return Ok("Message not found");
        }

    }
}

//namespace MessageApi.Controller
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class MessageController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly IMessageService _messageService;

//        public MessageController(IMapper mapper, IMessageService messageService)
//        {
//            _mapper = mapper;
//            _messageService = messageService;
//        }

//        [Authorize(Roles = "Administrator, User")]
//        [HttpGet("get")]
//        public ActionResult GetNewMessage()
//        {
//            var senderEmail = GetUserEmailFromToken().GetAwaiter().GetResult();

//            var response = _messageService.GetNewMessages(senderEmail);
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok(response.Messages);
//        }
//        [Authorize(Roles = "Administrator")]
//        [HttpPost("send")]
//        public ActionResult SendMessage(string recipientEmail, string text)
//        {
//            var senderEmail = GetUserEmailFromToken().GetAwaiter().GetResult();

//            var message = new MessageModel
//            {
//                RecipientEmail = recipientEmail,
//                SenderEmail = senderEmail,
//                Text = text
//            };

//            var response = _messageService.SendMessage(message);
//            if (!response.IsSuccess)
//                return BadRequest(response.Errors.FirstOrDefault().Message);

//            return Ok(response.Messages);
//        }

//        private async Task<string> GetUserEmailFromToken()
//        {
//            var token = await HttpContext.GetTokenAsync("access_token");

//            var handler = new JwtSecurityTokenHandler();
//            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
//            var claim = jwtToken!.Claims.Single(x => x.Type.Contains("emailaddress"));

//            return claim.Value;

//        }
//    }
//}