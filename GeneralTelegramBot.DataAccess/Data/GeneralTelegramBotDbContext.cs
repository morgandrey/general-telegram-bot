using GeneralTelegramBot.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace GeneralTelegramBot.DataAccess.Data
{
    public partial class GeneralTelegramBotDbContext : DbContext
    {
        public GeneralTelegramBotDbContext()
        {
        }

        public GeneralTelegramBotDbContext(DbContextOptions<GeneralTelegramBotDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.HasIndex(e => e.MessageId, "Message_message_id_uindex")
                    .IsUnique();

                entity.Property(e => e.MessageId).HasColumnName("message_id");

                entity.Property(e => e.MessageContent).HasColumnName("message_content");

                entity.Property(e => e.MessageCreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("message_creation_date");

                entity.Property(e => e.MessageUserId).HasColumnName("message_user_id");

                entity.Property(e => e.SaveUserId).HasColumnName("save_user_id");

                entity.HasOne(d => d.MessageUser)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.MessageUserId)
                    .HasConstraintName("Message_User_user_id_fk");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.HasIndex(e => e.PhotoId, "Photo_photo_id_uindex")
                    .IsUnique();

                entity.Property(e => e.PhotoId).HasColumnName("photo_id");

                entity.Property(e => e.PhotoCreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("photo_creation_date");

                entity.Property(e => e.PhotoHash).HasColumnName("photo_hash");

                entity.Property(e => e.PhotoSource).HasColumnName("photo_source");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Photo_User_user_id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.UserId, "User_user_id_uindex")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserLogin).HasColumnName("user_login");

                entity.Property(e => e.UserName).HasColumnName("user_name");

                entity.Property(e => e.UserSurname).HasColumnName("user_surname");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
