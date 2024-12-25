using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("AdminUsers")]
    public class AdminUser : BaseModel
    {
        [Key]
        public int Use_ID { get; set; }

        [Required(ErrorMessage = "Nhập tên tài khoản")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        [DisplayName("Tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có từ 8 đến 50 ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ in hoa, số và ký tự đặc biệt")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        [DisplayName("Tên người dùng")]
        public string? DisplayName { get; set; }

        [DisplayName("Ảnh đại diện")]
        public string? Avatar { get; set; }

        [DisplayName("Địa chỉ Email")]
        [Required(ErrorMessage = "Nhập địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự")]
        public string? Phone { get; set; }

        public string? Token { get; set; }

        [DisplayName("Trạng thái")]
        public int Status { get; set; }

        public string ShowStatus
        {
            get
            {
                return Status switch
                {
                    0 => "Chưa kích hoạt",
                    1 => "Đã khóa",
                    2 => "Đang hoạt động",
                    3 => "Đang hoạt động",
                    _ => "Không xác định"
                };
            }
        }

        public string ShowName
        {
            get
            {
                // Trả về DisplayName nếu có, nếu không trả về Username
                return !string.IsNullOrEmpty(DisplayName) ? DisplayName : Username;
            }
        }
    }
}