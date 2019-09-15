using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMod5.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Identification { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Book> Books { get; set; }
    }

}
