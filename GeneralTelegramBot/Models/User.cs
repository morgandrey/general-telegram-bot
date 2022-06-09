using System;
using System.Collections.Generic;

namespace GeneralTelegramBot.Models;

public partial class User
{
    public User()
    {
        Messages = new HashSet<Message>();
        Photos = new HashSet<Photo>();
    }

    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserSurname { get; set; } = null!;
    public string UserLogin { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<Photo> Photos { get; set; }
}