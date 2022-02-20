using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Dtos
{
    public class CreateProductDto
    {
        public CreateProductDto(string articleNumber, string name, string description, decimal price, string category)
        {
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string ArticleNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Category { get; set; }
    }
}
