using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Pno")]
        public string ProductNumber { get; set; }
        [Display(Name = "Standard Cost")]
        [DataType(DataType.Currency)]
        public decimal StandardCost { get; set; }
        [Display(Name = "List Price")]
        [DataType(DataType.Currency)]
        public decimal ListPrice { get; set; }
        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set;}
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [Timestamp]
        public byte[] Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
