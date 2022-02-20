using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApi.Dtos;
using WebShopApi.Entities;
using WebShopApi.Filter;

namespace WebShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly WebShopDbContext _dbContext;

        public ProductsController(ILogger<ProductsController> logger, WebShopDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [UseApiKey]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(string? category)
        {
            IQueryable<ProductEntity> productEntitiesQuery = _dbContext.Products.Include(p => p.Category);

            if(category is not null)
            {
                productEntitiesQuery = productEntitiesQuery.Where(p => p.Category.Name == category);
            }

            var productEntities = await productEntitiesQuery.ToListAsync();

            List<ProductDto> productDtos = new List<ProductDto>();

            foreach (var productEntity in productEntities)
            {
                var productDto = new ProductDto(
                    productEntity.Id,
                    productEntity.ArticleNumber,
                    productEntity.Name,
                    productEntity.Description,
                    productEntity.Price,
                    productEntity.Category.Name);

                productDtos.Add(productDto);
            }

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        [UseApiKey]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productEntity = await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity is null)
                return NotFound();

            var productDto = new ProductDto(
                productEntity.Id,
                productEntity.ArticleNumber,
                productEntity.Name,
                productEntity.Description,
                productEntity.Price,
                productEntity.Category.Name);

            return Ok(productDto);
        }

        [HttpPost]
        [UseAdminApiKey]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
        {
            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.ArticleNumber == createProductDto.ArticleNumber);

            if (productEntity is not null)
                return BadRequest($"Product with article number {createProductDto.ArticleNumber} already exist.");

            var productCategoryEntity = await _dbContext.ProductCategories.FirstOrDefaultAsync(p => p.Name == createProductDto.Category);

            ProductEntity newProductEntity;
            if(productCategoryEntity is null)
            {
                var newProductCategoryEntity = new ProductCategoryEntity(createProductDto.Category);
                newProductEntity = new ProductEntity(createProductDto.ArticleNumber, createProductDto.Description, createProductDto.Description, createProductDto.Price, newProductCategoryEntity);
            }
            else
            {
                newProductEntity = new ProductEntity(createProductDto.ArticleNumber, createProductDto.Description, createProductDto.Description, createProductDto.Price, productCategoryEntity.Id);
            }

            await _dbContext.Products.AddAsync(newProductEntity);
            await _dbContext.SaveChangesAsync();

            var productDto = new ProductDto(
                newProductEntity.Id,
                newProductEntity.ArticleNumber,
                newProductEntity.Name,
                newProductEntity.Description,
                newProductEntity.Price,
                newProductEntity.Category.Name);

            return CreatedAtAction("GetProduct", new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        [UseAdminApiKey]
        public async Task<ActionResult> UpdateProduct(int id, CreateProductDto createProductDto)
        {
            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity is null)
                return BadRequest("Product do not exist.");

            var productCategoryEntity = await _dbContext.ProductCategories.FirstOrDefaultAsync(p => p.Name == createProductDto.Category);
            
            if (productCategoryEntity is null)
            {
                var newProductCategoryEntity = new ProductCategoryEntity(createProductDto.Category);
                productEntity.ArticleNumber = createProductDto.ArticleNumber;
                productEntity.Name = createProductDto.Name;
                productEntity.Description = createProductDto.Description;
                productEntity.Price = createProductDto.Price;
                productEntity.Category = newProductCategoryEntity;
            }
            else
            {
                productEntity.ArticleNumber = createProductDto.ArticleNumber;
                productEntity.Name = createProductDto.Name;
                productEntity.Description = createProductDto.Description;
                productEntity.Price = createProductDto.Price;
                productEntity.ProductCategoryEntityId = productCategoryEntity.Id;
            }

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [UseAdminApiKey]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity is null)
                return BadRequest("Product do not exist.");

            _dbContext.Products.Remove(productEntity);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
