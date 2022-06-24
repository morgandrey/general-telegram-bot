using System;
using System.Collections.Generic;

namespace GeneralTelegramBot.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            MessageMessageUsers = new HashSet<Message>();
            MessageSaveUsers = new HashSet<Message>();
            Photos = new HashSet<Photo>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserSurname { get; set; }
        public string UserLogin { get; set; } = null!;

        public virtual ICollection<Message> MessageMessageUsers { get; set; }
        public virtual ICollection<Message> MessageSaveUsers { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
