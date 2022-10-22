using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? BooksBorrowed { get; set; }
        public string? OIB { get; set; }

    }
}
