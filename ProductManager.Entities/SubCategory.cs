using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Entities
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [Timestamp]
        public byte[] Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
