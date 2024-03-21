
namespace MainMailMessageApp.Repository
{
    public class MailMessageRepository : IMailMessageRepository
    {
       readonly HttpClient client = new HttpClient();
        //public void DeleteMessage(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool CheckUser(string name, string password)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> ExistUnreadedMessages(string receiverName)
        {
            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7126/Message/GetMessages?userName={receiverName}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if ( responseBody != null)
            {
                return true;
            }
            if (responseBody == null)
            {         
                return false;
            }
            throw new Exception("Unknown response");
        }

        //bool IMailMessageRepository.ExistUnreadedMessages(string receiverName)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> GetAllMessages(string receiverName)
        //{
        //    using HttpResponseMessage sponse = await client.GetAsync($"https://localhost:7126/Message/GetMessages?userName={receiverName}");

        //}
        //public List<Message> GetUnreadMessages(string receiverName)
        //{
        //    throw new NotImplementedException();
        //}

        //public Guid SendMessage(string text, string senderName, string receiverName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
