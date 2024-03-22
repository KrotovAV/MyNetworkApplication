using DataBaseMessage;
using DataBaseMessage.BD;

namespace MessageService.Repository
{
    public interface IMessageRepository
    {
        public Guid SendMessage(string text, string senderName, string receiverName);
        public List<Message> GetAllMessages(string receiverName);
        public List<Message> GetUnreceivedMessages(string receiverName);
        public bool DeleteMessage(Guid Id);
    }
}