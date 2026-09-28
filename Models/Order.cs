using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RoyalPerfume.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "رقم هاتف بديل")]
        public string? AlternatePhone { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [Display(Name = "العنوان التفصيلي")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [Display(Name = "المحافظة")]
        public string Governorate { get; set; } = string.Empty;

        [Display(Name = "المنطقة")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "طريقة الدفع مطلوبة")]
        [Display(Name = "طريقة الدفع")]
        public string PaymentMethod { get; set; } = "cash";

        [Display(Name = "ملاحظات إضافية")]
        public string? Notes { get; set; }

        [Display(Name = "صورة إيصال الدفع")]
        public IFormFile? ReceiptImage { get; set; }

        public CartViewModel Cart { get; set; } = new();
    }

    public class Order
    {
        public int Id { get; set; }   // PK في قاعدة البيانات

        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string AlternatePhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Governorate { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // Navigation property → OrderItems table
        public List<OrderItem> Items { get; set; } = new();

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public string ReceiptImagePath { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";
    }
}
