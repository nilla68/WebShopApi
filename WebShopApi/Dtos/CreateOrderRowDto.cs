using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos
{
    public class CreateOrderRowDto
    {
        public CreateOrderRowDto(int quantity, string articleNumber)
        {
            Quantity = quantity;
            ArticleNumber = articleNumber;
        }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ArticleNumber { get; set; }

    }
}
