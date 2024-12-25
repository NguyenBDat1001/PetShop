using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Banners")]
    public class Banner : BaseModel
    {
        [Key]
        public int Ban_ID { get; set; }

        [Required(ErrorMessage = "Nhập tiêu đề")]
        [StringLength(50, ErrorMessage = "Tiêu đề không được vượt quá 50 ký tự.")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [DisplayName("Hình ảnh")]
        public string? Image { get; set; }

        [DisplayName("Đường dẫn")]
        [StringLength(200, ErrorMessage = "Đường dẫn không được vượt quá 200 ký tự.")]
        public string? Url { get; set; }

        [Required(ErrorMessage = "Nhập số thứ tự")]
        [Range(1, 20, ErrorMessage = "Thứ tự hiển thị phải nằm trong khoảng từ 1 đến 20.")]
        [DisplayName("Số Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [Required(ErrorMessage = "Chọn vị trí")]
        [DisplayName("Vị trí hiển thị")]
        public int DisplayPosition { get; set; }

        public string ShowPosition
        {
            get
            {
                return DisplayPosition switch
                {
                    0 => "Đầu trang",
                    1 => "Giữa trang",
                    2 => "Cuối trang",
                    _ => "Không xác định"
                };
            }
        }
    }
}