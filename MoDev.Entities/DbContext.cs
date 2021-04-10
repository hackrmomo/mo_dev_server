using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using System;

namespace MoDev.Entities
{
    public class MoDevDbContext : DbContext
    {
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<Photograph> Photographs { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                Environment.GetEnvironmentVariable("MYSQL_CS")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}