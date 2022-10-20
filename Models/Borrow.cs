﻿using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

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


        public string UserId { get; set; }
        [ForeignKey("BorrowId")]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
