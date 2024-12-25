namespace PetShop.Areas.Admin.DTOs.request
{
    public class ProductDto
    {
        public int Pro_ID { get; set; }

        public int Cat_ID { get; set; }

        public int Typ_ID { get; set; }

        public IFormFile? Avatar { get; set; }

        public string Name { get; set; }

        public string? Brand { get; set; }

        public string? Intro { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public double? Discount { get; set; }

        public string Unit { get; set; }

        public double? Rate { get; set; }

        public string? Description { get; set; }

        public string? Details { get; set; }
    }
}