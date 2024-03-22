

namespace MainMailMessageApp.Repository
{
    public interface IMailMessageRepository
    {
        public bool ExistUnreadedMessages(string receiverName);
    }
        //public bool CheckUser(string name, string password);
        //public Guid SendMessage(string text, string senderName, string receiverName);
        //public List<Message> GetAllMessages(string receiverName);
        //public List<Message> GetUnreadMessages(string receiverName);
        //public void DeleteMessage(string name);


  
}
