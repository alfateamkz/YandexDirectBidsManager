using BidsManager.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BidsManager.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            if (Database.EnsureCreated())
                Database.Migrate();
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CampaignRule> CampaignRules { get; set; }
        public DbSet<YandexDirectAccount> YandexDirectAccounts  { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                       "server=localhost;" +
                       "user=root;" +
                       "password=H2c7V7p6;" +
                       "port=3306;" +
                       "database=yandexdirect;" +
                       "Convert Zero Datetime= true;",
                       new MySqlServerVersion(new Version(8, 0, 11)),
                       o =>
                       {
                           o.EnableRetryOnFailure(100);
                       });
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YandexDirectAccount>().HasMany(c => c.CampaignRules).WithOne(e => e.Owner);
            modelBuilder.Entity<UserModel>().HasMany(c => c.YandexDirectAccounts).WithOne(e => e.Owner);
        }
    }
}