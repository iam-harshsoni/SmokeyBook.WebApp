using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository categoryRepository { get; }
        IProductRepository productRepository { get; }

        void Save();

    }
}
