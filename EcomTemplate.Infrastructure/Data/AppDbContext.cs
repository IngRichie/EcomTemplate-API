using Microsoft.EntityFrameworkCore;
using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // =======================
    // CORE
    // =======================
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Category> Categories => Set<Category>();

    // =======================
    // CUSTOMER / PROFILE
    // =======================
    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();

    // =======================
    // CART
    // =======================
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    // =======================
    // ORDERS
    // =======================
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderAddress> OrderAddresses => Set<OrderAddress>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    // =======================
    // DELIVERY / FAVORITES
    // =======================
    public DbSet<DeliveryAddress> DeliveryAddresses => Set<DeliveryAddress>();
    public DbSet<Favourites> Favorites => Set<Favourites>();
    public DbSet<CheckoutSettings> CheckoutSettings => Set<CheckoutSettings>();

    // =======================
    // HOME / UI
    // =======================
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<HomeSection> HomeSections => Set<HomeSection>();

    public DbSet<HomeSectionProduct> HomeSectionProducts => Set<HomeSectionProduct>();

    // =======================
    // INVENTORY
    // =======================


    // =======================
    // REFERRALS
    // =======================


            public DbSet<Payment> Payments => Set<Payment>();
public DbSet<ProductImage> ProductImages => Set<ProductImage>();
public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

public DbSet<CustomerAuth> CustomerAuths => Set<CustomerAuth>();
public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


}
