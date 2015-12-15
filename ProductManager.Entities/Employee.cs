using ProductManager.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public PersonTitle Title { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [NotMapped] 
        public string FullName => String.Format("{0} {1}", FirstName, LastName); //Adicionado para obter de forma fácil o primeiro e último nome
        [EmailAddress]
        public string Email { get; set; } //Adicionado Email para identificar o utilizador
        [Timestamp]
        public byte[] Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
