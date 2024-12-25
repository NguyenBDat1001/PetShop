using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    [Table("Orders")]
    public class Order : BaseModel
    {
        [Key]
        [DisplayName("Mã đơn hàng")]
        public int Ord_ID { get; set; }

        [DisplayName("Thành viên")]
        public int? Mem_ID { get; set; }

        [DisplayName("Ngày đặt hàng")]
        public DateTime? OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Nhập tên khách hàng")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        [DisplayName("Tên khách hàng")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Nhập số điện thoại")]
        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Nhập địa chỉ")]
        [DisplayName("Địa chỉ")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Address { get; set; }

        [DisplayName("Tổng tiền")]
        public decimal TotalPrice { get; set; }

        [DisplayName("Giảm giá(%)")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100%.")]
        public double? Discount { get; set; }

        [DisplayName("Hình thức thanh toán")]
        public string PaymentMethod { get; set; }

        [DisplayName("Trạng thái thanh toán")]
        public bool IsPaid { get; set; }

        [DisplayName("Ghi chú")]
        public string? Note { get; set; }

        [DisplayName("Trạng thái")]
        public int? Status { get; set; }

        [ForeignKey("Mem_ID")]
        public virtual Member? Member { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

        public string ShowStatus
        {
            get
            {
                return Status switch
                {
                    0 => "Chưa giao hàng",
                    1 => "Đã giao hàng",
                    2 => "Đã hủy",
                    _ => "Không xác định"
                };
            }
        }

        public string ShowIsPaid => IsPaid ? "Đã thanh toán" : "Chưa thanh toán";

        public string OrderDateVN => OrderDate.HasValue
           ? OrderDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("vi-VN"))
           : "N/A";
    }
}