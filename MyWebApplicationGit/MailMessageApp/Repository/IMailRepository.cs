using MailApp_API_Gateway.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MailApp_API_Gateway.Repository
{
    public interface IMailRepository
    {
        public bool Login(string name, string password);
        public bool UserExists(string name);
        public void DeleteMessage(Guid guid);
        public Guid SendMessage(string text, string senderName, string receiverName);
        public List<MessageDto> GetAllMessages(string receiverName);
        public List<MessageDto> GetUnReceivedMessages(string receiverName);
    }
}
