using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Library.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [ForeignKey("PublisherId")]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
