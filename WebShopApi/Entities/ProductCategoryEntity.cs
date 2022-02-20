using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Entities
{
    public class ProductCategoryEntity
    {
        public ProductCategoryEntity(string name)
        {
            Name = name;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public IEnumerable<ProductEntity> Products { get; set; }
    }
}
