using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using fortune_api.LoadBoard.Models;
using fortune_api.Models.Auth;

namespace fortune_api.Persistence
{
    public class FortuneDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<EmailAuth> EmailAuths { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Load> Loads { get; set; }

        public FortuneDbContext()
            : base("FortuneDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}