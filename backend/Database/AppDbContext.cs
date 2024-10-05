using Database.ModelsConfigrantions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        IConfigurationRoot conf = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = conf.GetSection("constr").Value;

        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Club> Clubs { get; set; }

    public DbSet<ClubMember> ClubMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        new UserConfig().Configure(modelBuilder.Entity<User>());

        new ClubConfig().Configure(modelBuilder.Entity<Club>());

        new ClubMemberConfig().Configure(modelBuilder.Entity<ClubMember>());
    }
}