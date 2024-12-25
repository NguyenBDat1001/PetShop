using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Features")]
    public class Feature : BaseModel
    {
        [Key]
        public int Fea_ID { get; set; }

        [Required(ErrorMessage = "Nhập tên hoặc mã biểu tượng")]
        [StringLength(50, ErrorMessage = "Biểu tượng không được vượt quá 50 ký tự.")]
        [DisplayName("Biểu tượng")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Nhập tiêu đề")]
        [StringLength(50, ErrorMessage = "Tiêu đề không được vượt quá 50 ký tự.")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nhập Phụ đề")]
        [StringLength(200, ErrorMessage = "Phụ đề không được vượt quá 200 ký tự.")]
        [DisplayName("Phụ đề")]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "Nhập số thứ tự")]
        [Range(1, 20, ErrorMessage = "Thứ tự hiển thị phải nằm trong khoảng từ 1 đến 20.")]
        [DisplayName("Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }
    }
}