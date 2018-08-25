namespace ChoresAndFulfillment.Data
{
    public class Message
    {
        public int Id { get; set; }

        public string MessageSenderId { get; set; }
        public User MessageSender { get; set; }
        public string MessageReceiverId { get; set; }
        public User MessageReceiver { get; set; }
    }
}