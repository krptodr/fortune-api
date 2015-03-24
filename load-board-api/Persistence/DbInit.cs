using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace load_board_api.Persistence
{
    public class DbInit : CreateDatabaseIfNotExists<LoadBoardDbContext>
    {
        protected override void Seed(LoadBoardDbContext context)
        {
            base.Seed(context);
        }
    }
}