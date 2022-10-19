using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        [NotMapped]
        public string? BookTitle { get; set; }
        [NotMapped]
        public string? AuthorName { get; set; }

    }
}
