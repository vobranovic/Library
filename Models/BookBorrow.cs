using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class BookBorrow
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BorrowId { get; set; }

        [NotMapped]
        public string? BookTitle { get; set; }
        
    }
}
