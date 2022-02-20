using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos
{
    public class CreateOrderDto
    {
        public CreateOrderDto(string name, AddressDto address, IEnumerable<CreateOrderRowDto> orderRows)
        {
            Name = name;
            Address = address;
            OrderRows = orderRows;
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public AddressDto Address { get; set; }

        public IEnumerable<CreateOrderRowDto> OrderRows { get; set; }
    }
}
