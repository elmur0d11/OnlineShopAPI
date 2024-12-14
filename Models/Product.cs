using System.ComponentModel.DataAnnotations;

namespace OnlineShopAPIFull.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
