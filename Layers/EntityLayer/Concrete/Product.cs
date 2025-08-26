using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; }
        [StringLength(150)]
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        [StringLength(300)]
        public string ProductImage { get; set; }
        public bool ProductStatus { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
