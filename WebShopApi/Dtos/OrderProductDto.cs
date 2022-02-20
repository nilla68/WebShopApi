using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Dtos
{
    public class OrderProductDto
    {
        public OrderProductDto(string articleNumber, string name, decimal price)
        {
            ArticleNumber = articleNumber;
            Name = name;
            Price = price;
        }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string ArticleNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
