// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebShopApi;

#nullable disable

namespace WebShopApi.Migrations
{
    [DbContext(typeof(WebShopDbContext))]
    [Migration("20220220190533_WebShop")]
    partial class WebShop
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebShopApi.Entities.AddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("WebShopApi.Entities.ContactInformationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserEntityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserEntityId")
                        .IsUnique();

                    b.ToTable("ContactInformation");
                });

            modelBuilder.Entity("WebShopApi.Entities.OrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressEntityId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressEntityId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebShopApi.Entities.OrderRowEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("OrderEntityId")
                        .HasColumnType("int");

                    b.Property<int>("ProductEntityId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderEntityId");

                    b.HasIndex("ProductEntityId");

                    b.ToTable("OrderRows");
                });

            modelBuilder.Entity("WebShopApi.Entities.ProductCategoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("WebShopApi.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ArticleNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<int>("ProductCategoryEntityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryEntityId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebShopApi.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebShopApi.Entities.ContactInformationEntity", b =>
                {
                    b.HasOne("WebShopApi.Entities.UserEntity", null)
                        .WithOne("ContactInformation")
                        .HasForeignKey("WebShopApi.Entities.ContactInformationEntity", "UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebShopApi.Entities.OrderEntity", b =>
                {
                    b.HasOne("WebShopApi.Entities.AddressEntity", "Address")
                        .WithMany()
                        .HasForeignKey("AddressEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WebShopApi.Entities.OrderRowEntity", b =>
                {
                    b.HasOne("WebShopApi.Entities.OrderEntity", null)
                        .WithMany("OrderRows")
                        .HasForeignKey("OrderEntityId");

                    b.HasOne("WebShopApi.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebShopApi.Entities.ProductEntity", b =>
                {
                    b.HasOne("WebShopApi.Entities.ProductCategoryEntity", "Category")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebShopApi.Entities.UserEntity", b =>
                {
                    b.HasOne("WebShopApi.Entities.AddressEntity", "Address")
                        .WithMany("Users")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WebShopApi.Entities.AddressEntity", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("WebShopApi.Entities.OrderEntity", b =>
                {
                    b.Navigation("OrderRows");
                });

            modelBuilder.Entity("WebShopApi.Entities.ProductCategoryEntity", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebShopApi.Entities.UserEntity", b =>
                {
                    b.Navigation("ContactInformation")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
