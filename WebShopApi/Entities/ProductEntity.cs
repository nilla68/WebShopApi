using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Entities
{
    public class ProductEntity
    {
        public ProductEntity(string articleNumber, string name, string description, decimal price, ProductCategoryEntity category)
        {
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        public ProductEntity(string articleNumber, string name, string description, decimal price, int productCategoryEntityId)
        {
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Price = price;
            ProductCategoryEntityId = productCategoryEntityId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ArticleNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public ProductCategoryEntity Category { get; set; }

        public int ProductCategoryEntityId { get; set; }
    }
}
