using load_board_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace load_board_api.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepo<Value> ValueRepo { get; set; }

        void Save();
    }
}
