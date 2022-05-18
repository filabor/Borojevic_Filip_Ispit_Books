using System.ComponentModel.DataAnnotations;

namespace Ispit.Books.Models
{
    public class Author
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(5000)]
        public string? Bio { get; set; }

    }
}
