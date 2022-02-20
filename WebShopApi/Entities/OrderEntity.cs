using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Entities
{
    public class OrderEntity
    {
        public OrderEntity()
        {

        }
        public OrderEntity(string name, DateTime orderDate, OrderStatus status, AddressEntity address, ICollection<OrderRowEntity> orderRows)
        {
            Name = name;
            OrderDate = orderDate;
            Status = status;
            Address = address;
            OrderRows = orderRows;
        }

        public OrderEntity(string name, DateTime orderDate, OrderStatus status, int addressEntityId, ICollection<OrderRowEntity> orderRows)
        {
            Name = name;
            OrderDate = orderDate;
            Status = status;
            AddressEntityId = addressEntityId;
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

        public AddressEntity Address { get; set; }

        public int AddressEntityId { get; set; }

        public ICollection<OrderRowEntity> OrderRows { get; set; }
    }
}
