using DataBaseMessage.BD;
using Microsoft.EntityFrameworkCore;


namespace DataBaseMessage;

public class MessageContext: DbContext {

    public virtual DbSet<Message> Messages { get; set; }
    public MessageContext()
    {

    }
    public MessageContext(DbContextOptions dbc) : base(dbc)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build();

        //optionsBuilder.UseMySql(config.GetConnectionString("Connection"),
        //    new MySqlServerVersion(new Version(8, 0, 11)));

        optionsBuilder.UseLazyLoadingProxies().
                UseNpgsql(config.GetConnectionString("Connection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.Text)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.IsReceived)
                .IsRequired();
        });
    }
}