using Microsoft.EntityFrameworkCore;
using SmokeyBook.DataAccess.Data;
using SmokeyBook.DataAccess.Repository.IRepository;
using SmokeyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly ApplicationDBContext _db;

        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            //_db.Products.Update(obj);

            var objFromDB = _db.Products.FirstOrDefault(x=>x.Id== obj.Id);

            if (objFromDB != null)
            {
                objFromDB.Title = obj.Title;
                objFromDB.Description = obj.Description;
                objFromDB.Author = obj.Author;
                objFromDB.Category = obj.Category;
                objFromDB.ISBN = obj.ISBN;
                objFromDB.Price = obj.Price;
                objFromDB.Price50 = obj.Price50;
                objFromDB.Price100 = obj.Price100;
                objFromDB.ListPrice = obj.ListPrice;

                if (obj.ImageURL != null)
                {
                    objFromDB.ImageURL = obj.ImageURL;
                }
            }
        }
    }
}
