using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Entities
{
    public class OrderRowEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ProductEntity Product { get; set; }

        public int ProductEntityId { get; set; }
    }
}
