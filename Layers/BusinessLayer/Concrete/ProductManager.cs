using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productdal;

        public ProductManager(IProductDal productdal)
        {
            _productdal = productdal;
        }

        public List<Product> GetList()
        {
            return _productdal.List();
        }

        public Product GetProductById(int productId)
        {
            return _productdal.Get(x => x.ProductID == productId);
        }
        public List<Product> GetProductByCategory(int categoryId)
        {
            return _productdal.List(x => x.CategoryID == categoryId);
        }

        public void ProductAdd(Product product)
        {
            _productdal.Add(product);
        }

        public void ProductDelete(Product product)
        {
            _productdal.Delete(product);
        }

        public void ProductUpdate(Product product)
        {
            _productdal.Update(product);
        }
    }
}
