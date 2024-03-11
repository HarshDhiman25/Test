using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApplicationconsoleAppTest.Models
{
    public class Student
    {
        
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        
        public required string Grade { get; set; }
    }
}
