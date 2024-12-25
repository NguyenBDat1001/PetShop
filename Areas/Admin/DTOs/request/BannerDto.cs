namespace PetShop.Areas.Admin.DTOs.request
{
    public class BannerDto
    {
        public int Ban_ID { get; set; }

        public string Title { get; set; }

        public IFormFile? Image { get; set; }

        public int DisplayPosition { get; set; }

        public string? Url { get; set; }

        public int DisplayOrder { get; set; }
    }
}