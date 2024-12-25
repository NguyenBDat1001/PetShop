using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Articles")]
    public class Article : BaseModel
    {
        [Key]
        public string Art_ID { get; set; }

        [Required(ErrorMessage = "Nhập tiêu đề")]
        [StringLength(50, ErrorMessage = "Tiêu đề không được vượt quá 50 ký tự.")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [DisplayName("Ảnh bài viết")]
        public string? Avatar { get; set; }

        [Required(ErrorMessage = "Nhập Nội dung")]
        [DisplayName("Nội dung")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Nhập tên tác giả")]
        [StringLength(50, ErrorMessage = "Tên tác giả không được vượt quá 50 ký tự.")]
        [DisplayName("Tác giả")]
        public string Author { get; set; }
    }
}