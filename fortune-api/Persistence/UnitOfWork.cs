using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fortune_api.Models.LoadBoard;
using fortune_api.Models.Auth;

namespace fortune_api.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private FortuneDbContext context;
        private bool disposed;

        public IRepo<UserProfile> UserProfileRepo { get; set; }
        public IRepo<Permission> PermissionRepo { get; set; }
        public IRepo<EmailAuth> EmailAuthRepo { get; set; }
        public IRepo<Location> LocationRepo { get; set; }
        public IRepo<Trailer> TrailerRepo { get; set; }
        public IRepo<Load> LoadRepo { get; set; }

        public UnitOfWork(
            FortuneDbContext context,
            IRepo<Location> locationRepo,
            IRepo<Trailer> trailerRepo,
            IRepo<Load> loadRepo,
            IRepo<EmailAuth> emailAuthRepo,
            IRepo<UserProfile> userProfileRepo,
            IRepo<Permission> permissionRepo
        )
        {
            this.disposed = false;
            this.context = context;
            this.LocationRepo = locationRepo;
            this.TrailerRepo = trailerRepo;
            this.LoadRepo = loadRepo;
            this.EmailAuthRepo = emailAuthRepo;
            this.UserProfileRepo = userProfileRepo;
            this.PermissionRepo = permissionRepo;
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