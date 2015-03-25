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
        IRepo<TestObject> TestObjectRepo { get; set; }

        void Save();
    }
}
