namespace MailApp_API_Gateway.Dto
{

    public class MessageDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Text { get; set; }
        public bool IsReceived { get; set; }
        public DateTime DateTime { get; set; }
    }
}