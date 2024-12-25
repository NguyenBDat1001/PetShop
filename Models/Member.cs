using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Members")]
    public class Member : BaseModel
    {
        [Key]
        public int Mem_ID { get; set; }

        [Required(ErrorMessage = "Nhập tên tài khoản")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có từ 8 đến 50 ký tự.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ in hoa, số và ký tự đặc biệt.")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nhập tên người dùng")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        [DisplayName("Tên người dùng")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Chọn giới tính")]
        [DisplayName("Giới tính")]
        public bool Gender { get; set; }

        [DisplayName("Ảnh đại diện")]
        public string? Avatar { get; set; }

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Nhập địa chỉ Email")]
        [DisplayName("Địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(100, ErrorMessage = "Địa chỉ không được vượt quá 100 ký tự.")]
        public string? Address { get; set; }

        [DisplayName("Trạng thái")]
        public int Status { get; set; } = 0;

        public virtual ICollection<Order>? Orders { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
    }
}