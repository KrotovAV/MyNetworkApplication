using MailApp_API_Gateway.Dto;

namespace MailApp_API_Gateway.Repository
{
    public class MailRepository : IMailRepository
    {
        public void DeleteMessage(Guid guid)
        {
            throw new NotImplementedException();
        }

        public List<MessageDto> GetAllMessages(string receiverName)
        {
            throw new NotImplementedException();
        }

        public List<MessageDto> GetUnReceivedMessages(string receiverName)
        {
            throw new NotImplementedException();
        }

        public bool Login(string name, string password)
        {
            throw new NotImplementedException();
        }

        public Guid SendMessage(string text, string senderName, string receiverName)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string name)
        {
            throw new NotImplementedException();
        }
    }
}
