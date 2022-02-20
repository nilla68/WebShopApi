using System.ComponentModel.DataAnnotations;
using WebShopApi.Entities;

namespace WebShopApi.Dtos
{
    public class OrderDto
    {
        public OrderDto(int id, string name, DateTime orderDate, OrderStatus status, AddressDto address, IEnumerable<OrderRowDto> orderRows)
        {
            Id = id;
            Name = name;
            OrderDate = orderDate;
            Status = status;
            Address = address;
            OrderRows = orderRows;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public AddressDto Address { get; set; }

        public IEnumerable<OrderRowDto> OrderRows { get; set; }
    }
}
