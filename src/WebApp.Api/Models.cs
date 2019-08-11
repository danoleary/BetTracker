using Microsoft.EntityFrameworkCore;
using System;

namespace WebApp.Api
{
    public class BetTrackerContext : DbContext
    {
        public BetTrackerContext(DbContextOptions<BetTrackerContext> options):base(options)
        { }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost;port=5432;database=bettrackerwebapp;username=bettrackerwebapp;password=password");
        }
    }

    public class User
    {
        public string UserId { get; set; }
        public Guid BookiesAggregateId { get; set; }
    }
}