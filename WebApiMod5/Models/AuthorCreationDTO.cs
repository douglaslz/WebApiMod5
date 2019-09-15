using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMod5.Models
{
    public class AuthorCreationDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
