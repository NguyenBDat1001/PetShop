using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Categories")]
    public class Category : BaseModel
    {
        [Key]
        public int Cat_ID { get; set; }

        [Required(ErrorMessage = "Nhập tên danh mục")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự.")]
        [DisplayName("Tên danh mục")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nhập số thứ tự")]
        [Range(1, 20, ErrorMessage = "Thứ tự hiển thị phải nằm trong khoảng từ 1 đến 20.")]
        [DisplayName("Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        [NotMapped]
        [DisplayName("Số lượng sản phẩm")]
        public int ProductCount => Products?.Count ?? 0;
    }
}