using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Settings")]
    public class Setting : BaseModel
    {
        [Key]
        public string Set_ID { get; set; }

        [Required(ErrorMessage = "Nhập tên")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        [DisplayName("Tên cài đặt")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nhập giá trị")]
        [StringLength(1000, ErrorMessage = "Giá trị không được vượt quá 1000 ký tự.")]
        [DisplayName("Giá trị")]
        public string Value { get; set; }
    }
}