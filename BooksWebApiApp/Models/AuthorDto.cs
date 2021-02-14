namespace WebApplication.Models
{
    public class AuthorDto
    {
        public int Id { get; set; }        
        public string Name { get; set; }

        public static explicit operator AuthorDto(Author author)
        {
            return new AuthorDto() { Id = author.Id, Name = author.Name };
        }
    }
}
