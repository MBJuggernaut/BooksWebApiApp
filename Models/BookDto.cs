using System.Collections.Generic;

namespace WebApplication.Models
{
    public class BookDto
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public virtual ICollection<AuthorDto> Authors { get; set; }

        public static explicit operator BookDto(Book book)
        {
            var bdto = new BookDto() { Id = book.Id, Name = book.Name, Authors = new List<AuthorDto>() };

            foreach (var author in book.Authors)
            {
                bdto.Authors.Add((AuthorDto)author);
            }

            return bdto;
        }
    }
}
