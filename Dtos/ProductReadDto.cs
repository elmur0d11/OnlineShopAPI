using System.ComponentModel.DataAnnotations;

namespace OnlineShopAPIFull.Dtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Balance { get; set; }

    }
}
