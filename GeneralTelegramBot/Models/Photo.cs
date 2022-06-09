using System;
using System.Collections.Generic;

namespace GeneralTelegramBot.Models;

public partial class Photo
{
    public int PhotoId { get; set; }
    public string PhotoHash { get; set; } = null!;
    public byte[] PhotoSource { get; set; } = null!;
    public DateTime PhotoCreationDate { get; set; }
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}