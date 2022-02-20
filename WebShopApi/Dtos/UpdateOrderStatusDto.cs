using System.ComponentModel.DataAnnotations;
using WebShopApi.Entities;

namespace WebShopApi.Dtos
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
