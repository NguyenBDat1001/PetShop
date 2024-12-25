using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Products")]
    public class Product : BaseModel
    {
        [Key]
        public int Pro_ID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        [DisplayName("Danh mục")]
        public int Cat_ID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại.")]
        [DisplayName("Loại sản phẩm")]
        public int Typ_ID { get; set; }

        [DisplayName("Ảnh sản phẩm")]
        public string? Avatar { get; set; }

        [Required(ErrorMessage = "Nhập Tên sản phẩm.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Xuất xứ không được vượt quá 50 ký tự.")]
        [DisplayName("Xuất xứ")]
        public string? Brand { get; set; }

        [StringLength(500, ErrorMessage = "Giới thiệu không được vượt quá 500 ký tự.")]
        [DisplayName("Giới thiệu")]
        public string? Intro { get; set; }

        [Required(ErrorMessage = "Nhập Giá tiền.")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá tiền phải là số và lớn hơn hoặc bằng 0.")]
        [DisplayName("Giá tiền")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Nhập số lượng.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải là số lớn hơn hoặc bằng 1.")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100%.")]
        [DisplayName("Giảm giá(%)")]
        public double? Discount { get; set; }

        [Required(ErrorMessage = "Nhập đơn vị.")]
        [StringLength(20, ErrorMessage = "Đơn vị không được vượt quá 20 ký tự.")]
        [DisplayName("Đơn vị")]
        public string Unit { get; set; }

        [Range(0, 5, ErrorMessage = "Đánh giá phải nằm trong khoảng từ 0 đến 5.")]
        [DisplayName("Đánh giá")]
        public double? Rate { get; set; }

        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [DisplayName("Chi tiết")]
        public string? Details { get; set; }

        [ForeignKey("Cat_ID")]
        public virtual Category? Category { get; set; }

        [ForeignKey("Typ_ID")]
        public virtual Type? Type { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}