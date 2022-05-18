using System.ComponentModel.DataAnnotations;

namespace Ispit.Books.Models
{
    public class Publisher
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

    }
}
