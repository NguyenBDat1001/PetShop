using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Reviews")]
    public class Review : BaseModel
    {
        [Key]
        public int Rev_ID { get; set; }

        [DisplayName("Khách hàng")]
        public int Mem_ID { get; set; }

        [DisplayName("Sản phẩm")]
        public int Pro_ID { get; set; }

        [Required(ErrorMessage = "Chọn đánh giá")]
        [DisplayName("Đánh giá")]
        public double Rate { get; set; }

        [DisplayName("Nội dung")]
        public string? Content { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [ForeignKey("Pro_ID")]
        public virtual Product? Product { get; set; }

        [ForeignKey("Mem_ID")]
        public virtual Member? Member { get; set; }
    }
}