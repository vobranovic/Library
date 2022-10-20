using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Library.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateBorrowed { get; set; }
        [Required]
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? DateReturned { get; set; }
        [NotMapped]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }


        public string UserId { get; set; }
        [ForeignKey("BorrowId")]
        public List<BookBorrow> BookBorrow { get; set; } = new List<BookBorrow>();
    }
}
