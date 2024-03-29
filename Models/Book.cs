﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public int Stock { get; set; }
        public int Available { get; set; }
        [Required]
        public DateTime Year { get; set; }
        [NotMapped]
        public string? Publisher { get; set; }

        public int PublisherId { get; set; }

        [ForeignKey("BookId")]
        public List<BookBorrow> BookBorrow { get; set; } = new List<BookBorrow>();

        [ForeignKey("BookId")]
        public List<BookAuthor> BookAuthor { get; set; } = new List<BookAuthor>();
    }
}
