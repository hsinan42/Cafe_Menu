using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IProductService
    {
        List<Product> GetList();
        void ProductAdd(Product product);
        void ProductUpdate(Product product);
        void ProductDelete(Product product);
        Product GetProductById(int productId);

    }
}
