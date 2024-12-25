using PetShop.Models;

namespace PetShop.Areas.Admin.DTOs.request
{
    public class OrderDto
    {
        public int Ord_ID { get; set; }

        public int? Mem_ID { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }

        public double? Discount { get; set; }

        public string PaymentMethod { get; set; }

        public bool IsPaid { get; set; }

        public string? Note { get; set; }

        public int? Status { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}