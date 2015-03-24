﻿using System;
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

        public IRepo<Value> ValueRepo { get; set; }

        public UnitOfWork(
            LoadBoardDbContext context,
            IRepo<Value> valueRepo
        )
        {
            this.disposed = false;
            this.ValueRepo = valueRepo;
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