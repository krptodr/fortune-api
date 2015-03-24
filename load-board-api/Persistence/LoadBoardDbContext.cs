using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using load_board_api.Models;

namespace load_board_api.Persistence
{
    public class LoadBoardDbContext : DbContext
    {
        public DbSet<Value> Values { get; set; }

        public LoadBoardDbContext()
            : base("LoadBoardDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}