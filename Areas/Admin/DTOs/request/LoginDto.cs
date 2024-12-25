using System.ComponentModel.DataAnnotations;

namespace PetShop.Areas.Admin.DTOs.request
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(30, ErrorMessage = "Tên tài khoản không hợp lệ")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có từ 8 đến 50 ký tự.")]

        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}