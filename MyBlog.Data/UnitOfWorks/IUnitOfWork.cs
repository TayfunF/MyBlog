using MyBlog.Core.Entities;
using MyBlog.Data.Repositories.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class, IEntityBase, new();

        Task<int> SaveAsync();
        int Save();
    }
}
