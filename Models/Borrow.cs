using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Library.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Date Borrowed")]
        public DateTime DateBorrowed { get; set; }
        [Required]
        [Display(Name = "Expected Return Date")]
        public DateTime ExpectedReturnDate { get; set; }
        [Display(Name = "Date Returned")]
        public DateTime? DateReturned { get; set; }

        [NotMapped]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [NotMapped]
        [Display(Name = "Books Borrowed")]
        public int BooksBorrowed { get; set; }


        public string UserId { get; set; }
        [ForeignKey("BorrowId")]
        public List<BookBorrow> BookBorrow { get; set; } = new List<BookBorrow>();
    }
}
