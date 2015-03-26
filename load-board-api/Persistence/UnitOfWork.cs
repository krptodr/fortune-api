using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using load_board_api.Models;

namespace load_board_api.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private LoadBoardDbContext context;
        private bool disposed;

        public IRepo<TestObject> TestObjectRepo { get; set; }
        public IRepo<Location> LocationRepo { get; set; }

        public UnitOfWork(
            LoadBoardDbContext context,
            IRepo<TestObject> testObjectRepo,
            IRepo<Location> locationRepo
        )
        {
            this.disposed = false;
            this.context = context;
            this.TestObjectRepo = testObjectRepo;
            this.LocationRepo = LocationRepo;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        # endregion
    }
}