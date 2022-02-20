using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApi.Dtos;
using WebShopApi.Entities;
using WebShopApi.Filter;

namespace WebShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly WebShopDbContext _dbContext;

        public OrdersController(ILogger<OrdersController> logger, WebShopDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orderEntities = await _dbContext.Orders.Include(o => o.Address).Include(o => o.OrderRows).ThenInclude(r => r.Product).ToListAsync();

            List<OrderDto> orderDtos = new List<OrderDto>();

            foreach (var orderEntity in orderEntities)
            {
                var addressDto = new AddressDto(
                    orderEntity.Address.Street,
                    orderEntity.Address.PostCode,
                    orderEntity.Address.City,
                    orderEntity.Address.Country);

                List<OrderRowDto> orderRowDtos = new List<OrderRowDto>();
                foreach (var orderRowEntity in orderEntity.OrderRows)
                {
                    var orderProductDto = new OrderProductDto(
                        orderRowEntity.Product.ArticleNumber,
                        orderRowEntity.Product.Name,
                        orderRowEntity.Product.Price);

                    var orderRowDto = new OrderRowDto(orderRowEntity.Id, orderRowEntity.Quantity, orderProductDto);
                    orderRowDtos.Add(orderRowDto);
                }

                var newOrderDto = new OrderDto(
                    orderEntity.Id,
                    orderEntity.Name,
                    orderEntity.OrderDate,
                    orderEntity.Status,
                    addressDto, orderRowDtos);

                orderDtos.Add(newOrderDto);
            }

            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var orderEntity = await _dbContext.Orders.Include(o => o.Address).Include(o => o.OrderRows).ThenInclude(r => r.Product).FirstOrDefaultAsync(o => o.Id == id);

            if (orderEntity is null)
                return NotFound();

            var addressDto = new AddressDto(
                orderEntity.Address.Street,
                orderEntity.Address.PostCode,
                orderEntity.Address.City,
                orderEntity.Address.Country);

            List<OrderRowDto> orderRowDtos = new List<OrderRowDto>();
            foreach (var orderRowEntity in orderEntity.OrderRows)
            {
                var orderProductDto = new OrderProductDto(
                            orderRowEntity.Product.ArticleNumber,
                            orderRowEntity.Product.Name,
                            orderRowEntity.Product.Price);

                var orderRowDto = new OrderRowDto(orderRowEntity.Id, orderRowEntity.Quantity, orderProductDto);
                orderRowDtos.Add(orderRowDto);
            }

            var newOrderDto = new OrderDto(
                orderEntity.Id,
                orderEntity.Name,
                orderEntity.OrderDate,
                orderEntity.Status,
                addressDto, orderRowDtos);

            return Ok(newOrderDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            List<OrderRowEntity> orderRowEntities = new List<OrderRowEntity>();

            foreach (var orderRowDto in createOrderDto.OrderRows)
            {
                var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.ArticleNumber == orderRowDto.ArticleNumber);

                if (productEntity is null)
                    return BadRequest();

                var orderRowEntity = new OrderRowEntity
                {
                    ProductEntityId = productEntity.Id,
                    Quantity = orderRowDto.Quantity
                };

                orderRowEntities.Add(orderRowEntity);
            }

            var addressEntity = await _dbContext.Address.FirstOrDefaultAsync(a => a.Street == createOrderDto.Address.Street && a.PostCode == createOrderDto.Address.PostCode);
            OrderEntity orderEntity;

            if (addressEntity is null)
            {
                addressEntity = new AddressEntity(createOrderDto.Address.Street, createOrderDto.Address.PostCode, createOrderDto.Address.City, createOrderDto.Address.Country);
                orderEntity = new OrderEntity(createOrderDto.Name, DateTime.Now, OrderStatus.Ordered, addressEntity, orderRowEntities);
            }
            else
            {
                orderEntity = new OrderEntity(createOrderDto.Name, DateTime.Now, OrderStatus.Ordered, addressEntity.Id, orderRowEntities);
            }

            _dbContext.Orders.Add(orderEntity);
            await _dbContext.SaveChangesAsync();


            var addressDto = new AddressDto(
                orderEntity.Address.Street,
                orderEntity.Address.PostCode,
                orderEntity.Address.City,
                orderEntity.Address.Country);

            List<OrderRowDto> orderRowDtos = new List<OrderRowDto>();
            foreach (var orderRowEntity in orderEntity.OrderRows)
            {
                var orderProductDto = new OrderProductDto(
                            orderRowEntity.Product.ArticleNumber,
                            orderRowEntity.Product.Name,
                            orderRowEntity.Product.Price);

                var orderRowDto = new OrderRowDto(orderRowEntity.Id, orderRowEntity.Quantity, orderProductDto);
                orderRowDtos.Add(orderRowDto);
            }

            var newOrderDto = new OrderDto(
                orderEntity.Id,
                orderEntity.Name,
                orderEntity.OrderDate,
                orderEntity.Status,
                addressDto, orderRowDtos);

            return CreatedAtAction("GetOrder", new { id = orderEntity.Id }, newOrderDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var orderEntity = await _dbContext.Orders.Include(o => o.OrderRows).FirstOrDefaultAsync(o => o.Id == id);

            if (orderEntity is null)
                return NotFound();

            foreach (var orderRowEntity in orderEntity.OrderRows)
            {
                _dbContext.OrderRows.Remove(orderRowEntity);
            }

            _dbContext.Orders.Remove(orderEntity);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/row")]
        [Authorize]
        public async Task<ActionResult> AddOrderRow(int id, CreateOrderRowDto orderRowDto)
        {
            var orderEntity = await _dbContext.Orders.Include(o => o.OrderRows).FirstOrDefaultAsync(o => o.Id == id);

            if (orderEntity is null)
                return BadRequest();

            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.ArticleNumber == orderRowDto.ArticleNumber);

            if (productEntity is null)
                return BadRequest();

            var orderRowEntity = new OrderRowEntity
            {
                ProductEntityId = productEntity.Id,
                Quantity = orderRowDto.Quantity
            };

            orderEntity.OrderRows.Add(orderRowEntity);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{orderId}/row/{rowId}")]
        [Authorize]
        public async Task<ActionResult> DeleteOrderRow(int orderId, int rowId)
        {
            var orderRowEntity = await _dbContext.Orders.Where(o => o.Id == orderId).SelectMany(o => o.OrderRows).Where(r => r.Id == rowId).FirstOrDefaultAsync();

            if (orderRowEntity is null)
                return BadRequest();

            _dbContext.OrderRows.Remove(orderRowEntity);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{orderId}/row/{rowId}")]
        [Authorize]
        public async Task<ActionResult> UpdateOrderRow(int orderId, int rowId, CreateOrderRowDto orderRowDto)
        {
            var orderRowEntity = await _dbContext.Orders.Where(o => o.Id == orderId).SelectMany(o => o.OrderRows).Where(r => r.Id == rowId).FirstOrDefaultAsync();

            if (orderRowEntity is null)
                return BadRequest();

            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.ArticleNumber == orderRowDto.ArticleNumber);

            if (productEntity is null)
                return BadRequest();

            orderRowEntity.ProductEntityId = productEntity.Id;
            orderRowEntity.Quantity = orderRowDto.Quantity;

            await _dbContext.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto updateOrderStatusDto)
        {
            var orderEntity = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (orderEntity is null)
                return BadRequest();

            orderEntity.Status = updateOrderStatusDto.Status;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
