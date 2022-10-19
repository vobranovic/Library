using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }        
        public int Stock { get; set; }
        [Required]
        public DateTime Year { get; set; }
        [NotMapped]
        public string? Publisher { get; set; }

        public int PublisherId { get; set; }
        public int? BorrowId { get; set; }

        [ForeignKey("BookId")]
        public List<BookAuthor> BookAuthor { get; set; } = new List<BookAuthor>();
    }
}
