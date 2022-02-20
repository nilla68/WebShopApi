using Microsoft.EntityFrameworkCore;
using WebShopApi.Entities;

namespace WebShopApi
{
    public class WebShopDbContext : DbContext
    {
        public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users => Set<UserEntity>();

        public DbSet<ContactInformationEntity> ContactInformation => Set<ContactInformationEntity>();

        public DbSet<AddressEntity> Address => Set<AddressEntity>();

        public DbSet<ProductEntity> Products => Set<ProductEntity>();

        public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();

        public DbSet<OrderEntity> Orders => Set<OrderEntity>();

        public DbSet<OrderRowEntity> OrderRows => Set<OrderRowEntity>();
    }
}
