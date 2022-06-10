namespace GeneralTelegramBot.DataAccess.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; } = null!;
        public DateTime MessageCreationDate { get; set; }
        public int MessageUserId { get; set; }
        public int SaveUserId { get; set; }

        public virtual User MessageUser { get; set; } = null!;
    }
}
