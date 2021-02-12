using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Author
    {
        [Key]        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
}
