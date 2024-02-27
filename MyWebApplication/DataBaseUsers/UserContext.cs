using DataBaseUsers.BD;
using DataBaseUsers.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataBaseUsers;

public class UserContext: DbContext {
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public UserContext()
    {

    }
    public UserContext(DbContextOptions dbc) : base(dbc)
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
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");
            entity.HasIndex(e => e.Name).IsUnique();

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Salt).HasColumnName("salt");

            entity.Property(e => e.RoleId).HasConversion<int>();
        });

        modelBuilder.Entity<Role>()
            .Property(e => e.RoleId)
            .HasConversion<int>();

        modelBuilder.Entity<Role>()
            .HasData(Enum.GetValues(typeof(RoleId))
            .Cast<RoleId>()
            .Select(e => new Role() 
                {
                    RoleId = e,
                    Name = e.ToString()
                }
            ));
    }
}