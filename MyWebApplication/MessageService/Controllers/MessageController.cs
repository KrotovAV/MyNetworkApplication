using MessageService.Model;
using MessageService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageRepository _messageRepository;

        public MessageController(IConfiguration configuration, IMessageRepository messageRepository)
        {
            _configuration = configuration;
            _messageRepository = messageRepository;
        }

        [HttpGet]
        [Route("GetAllMessages")]
        public IActionResult GetAllMessages(string userName)
        {
            var messages = _messageRepository.GetAllMessages(userName);
            return Ok(messages);
        }

        [HttpGet]
        [Route("GetUnreceivedMessages")]
        public IActionResult GetUnreceivedMessages(string userName)
        {
            var messages = _messageRepository.GetUnreceivedMessages(userName);
            return Ok(messages);
        }

        [HttpPost]
        [Route("SendMessage")]
        public IActionResult SendMessage([FromBody] MessageModel message)
        {
            var messageId = _messageRepository.SendMessage(
                message.Text,
                message.SenderName,
                message.ReceiverName);
            return Ok(messageId);
        }

        [HttpDelete]
        [Route("DeleteMessage")]
        public IActionResult DeleteMessage(Guid Id)
        {
            bool result = _messageRepository.DeleteMessage(Id);
            if(result == true) 
                return Ok($"Message id:{Id} delete");
            return Ok($"Message id:{Id} not found");
        }

    }
}