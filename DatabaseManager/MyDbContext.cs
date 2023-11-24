using FootParser.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootParser.DatabaseManager;

public class MyDbContext(string username, string password, string myServerAddress, string myPort, string myDataBase) : DbContext
{
    private readonly string _connectionString =
        $"Server={myServerAddress};Port={myPort};Database={myDataBase};User Id={username};Password={password};";

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<PlayerStat> PlayerStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .Ignore(p => p.FullName)
            .Ignore(p => p.Games)
            .Ignore(p => p.GoalsTotal)
            .Ignore(p => p.Goals)
            .Ignore(p => p.PenaltyGoals)
            .Ignore(p => p.Assists)
            .Ignore(p => p.YellowCards)
            .Ignore(p => p.RedCards)
            .Ignore(p => p.Motm)
            .Ignore(p => p.SeasonId);

        modelBuilder.Entity<PlayerStat>()
            .Ignore(p => p.Firstname)
            .Ignore(p => p.Lastname)
            .Ignore(p => p.Middlename);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}