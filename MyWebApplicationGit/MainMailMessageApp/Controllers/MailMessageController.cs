
using MainMailMessageApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MainMailMessageApp.Controllers
{
    [ApiController]
    [Route("[mailController]")]
    public class MailMessageController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly IMailMessageRepository _mailMessageRepository;

        //public MailMessageController(IConfiguration configuration, IMailMessageRepository mailMessageRepository)
        //public MailMessageController(IMailMessageRepository mailMessageRepository)
        //{
        //    //_configuration = configuration;
        //    _mailMessageRepository = mailMessageRepository;
        //}

        //[HttpPost]
        //[Route("GetUnreadedMessages")]

        //public async Task<bool> ExistUnreadedMessages(string receiverName)
        //{
        //    var messages = _mailMessageRepository.ExistUnreadedMessages(receiverName);
        //    return Ok(value: messages);
        //}

        //[HttpGet]
        //[Route("GetMessages")]

        //public IActionResult GetAllMessages(string userName)
        //{
        //    var messages = _messageRepository.GetAllMessages(userName);
        //    return Ok(messages);
        //}

        //[HttpPost]
        //[Route("SendMessage")]

        //public IActionResult SendMessage(MessageModel message)
        //{

        //    var sender = 1;
        //    var recever = 1;
        //    if (sender != null && recever != null)
        //    {
        //        var messageId = _messageRepository.SendMessage(
        //            message.Text,
        //            message.SenderName,
        //            message.ReceiverName
        //        );
        //        return Ok(messageId);
        //    }

        //    return NotFound("Sender or Receiver not fount in base");
        //}

    }
}