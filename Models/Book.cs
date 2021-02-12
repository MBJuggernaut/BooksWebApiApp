using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       // public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; }

        public Book()
        {
          //  Authors = new List<Author>();
        }
    }
}
