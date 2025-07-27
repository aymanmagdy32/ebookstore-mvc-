 using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled,
        Returned,
        Refund,
    };
}

