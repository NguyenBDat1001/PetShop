using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Types")]
    public class Type : BaseModel
    {
        [Key]
        public int Typ_ID { get; set; }

        [Required(ErrorMessage = "Nhập tên loại")]
        [StringLength(50, ErrorMessage = "Tên loại không được vượt quá 50 ký tự.")]
        [DisplayName("Tên loại")]
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