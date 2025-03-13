using System;
using System.Collections.Generic;

namespace Bestshop.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal ShippingFee { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
    }
}
