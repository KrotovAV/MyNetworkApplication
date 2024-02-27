using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp06S
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=MySQLavk;database=c_sharp_06_lesson;",
                new MySqlServerVersion(new Version(8, 0, 11)));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id).HasName("userPk");
                entity.HasIndex(x => x.FullName).IsUnique();

                entity.Property(e => e.FullName).HasColumnName("FullName").HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.HasKey(x => x.MessageId).HasName("messagePk");

                entity.Property(e => e.Text).HasColumnName("messageText");
                entity.Property(e => e.DateSend).HasColumnName("messageData");
                entity.Property(e => e.IsSent).HasColumnName("is_sent");
                entity.Property(e => e.MessageId).HasColumnName("id");

                entity.HasOne(x => x.UserTo).WithMany(m => m.MessagesTo).HasForeignKey(x => x.UserToId).HasConstraintName("messageToUserFk");
                entity.HasOne(x => x.UserFrom).WithMany(m => m.MessagesFrom).HasForeignKey(x => x.UserFromId).HasConstraintName("messageFromUserFk");
            });
        }
        public ChatContext()
        {

        }
        public ChatContext(DbContextOptions dbc) : base(dbc)
        {

        }
    }
}
