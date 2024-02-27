using DataBaseMessage.BD;

namespace DataBaseMessage.Repository;

public interface IMessageRepository {
    public Guid SendMessage(string text, string senderName, string receiverName);
    public List<Message> GetAllMessages(string receiverName);
   
}