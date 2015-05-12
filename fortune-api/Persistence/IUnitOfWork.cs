using fortune_api.LoadBoard.Models;
using fortune_api.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepo<UserProfile> UserProfileRepo { get; set; }
        IRepo<Permission> PermissionRepo { get; set; }
        IRepo<EmailAuth> EmailAuthRepo { get; set; }
        IRepo<Location> LocationRepo { get; set; }
        IRepo<Trailer> TrailerRepo { get; set; }
        IRepo<Load> LoadRepo { get; set; }

        void Save();
    }
}
