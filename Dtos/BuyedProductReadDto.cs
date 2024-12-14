namespace OnlineShopAPIFull.Dtos
{
    public class BuyedProductReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Balance { get; set; }

        public DateTime buyedDate {  get; set; }
    }
}
