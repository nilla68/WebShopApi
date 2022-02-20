using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos
{
    public class OrderRowDto
    {
        public OrderRowDto(int id, int quantity, OrderProductDto product)
        {
            Id = id;
            Quantity = quantity;
            Product = product;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        public OrderProductDto Product { get; set; }
    }
}
