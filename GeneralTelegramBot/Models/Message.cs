using System;
using System.Collections.Generic;

namespace GeneralTelegramBot.Models;

public partial class Message
{
    public int MessageId { get; set; }
    public string MessageContent { get; set; } = null!;
    public DateTime MessageCreationDate { get; set; }
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}