using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Menus")]
    public class Menu : BaseModel
    {
        [Key]
        public int Men_ID { get; set; }

        [DisplayName("Menu cha")]
        public int? Parent_ID { get; set; }

        [Required(ErrorMessage = "Nhập Tiêu đề")]
        [StringLength(50, ErrorMessage = "Tiêu đề không được vượt quá 50 ký tự.")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nhập đường dẫn")]
        [StringLength(200, ErrorMessage = "Đường dẫn không được vượt quá 200 ký tự.")]
        [DisplayName("Đường dẫn")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Nhập số thứ tự")]
        [Range(1, 20, ErrorMessage = "Thứ tự hiển thị phải nằm trong khoảng từ 1 đến 20.")]
        [DisplayName("Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [ForeignKey("Parent_ID")]
        public Menu? Parent { get; set; } // Điều hướng tới mục cha

        public virtual ICollection<Menu>? Children { get; set; } // Điều hướng tới các mục con

    }
}