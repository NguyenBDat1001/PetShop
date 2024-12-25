using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("OrderDetails")]
    public class OrderDetail : BaseModel
    {
        [Key]
        public int Ordd_ID { get; set; }

        public int Ord_ID { get; set; }

        [DisplayName("Sản phẩm")]
        public int Pro_ID { get; set; }

        [Required(ErrorMessage = "Nhập số lượng")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Giá tiền")]
        public decimal Price { get; set; }

        [DisplayName("Giá giảm")]
        public decimal DiscountPrice { get; set; }

        [ForeignKey("Ord_ID")]
        public virtual Order? Order { get; set; }

        [ForeignKey("Pro_ID")]
        public virtual Product? Product { get; set; }
    }
}