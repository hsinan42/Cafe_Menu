using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        [StringLength(50)]
        public string AdminName { get; set; }
        [StringLength(30)]
        public string AdminMail { get; set; }
        [StringLength(255)]
        public string AdminPassword { get; set; }
    }
}
