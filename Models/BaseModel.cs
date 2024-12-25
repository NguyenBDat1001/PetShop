using System.ComponentModel;

namespace PetShop.Models
{
    public class BaseModel
    {
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; }

        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }

        [DisplayName("Người cập nhật")]
        public string? UpdatedBy { get; set; }

        // Định dạng ngày theo kiểu Việt Nam
        public string CreatedDateVN => CreatedDate.HasValue
            ? CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("vi-VN"))
            : "N/A";

        public string UpdatedDateVN => UpdatedDate.HasValue
            ? UpdatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("vi-VN"))
            : "N/A";
    }
}