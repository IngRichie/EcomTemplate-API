using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // =======================
        // PRODUCT (FULL GRAPH)
        // =======================
        CreateMap<Product, ProductDTO>()
    .ForMember(d => d.CategoryName,
        o => o.MapFrom(s => s.Category.Name))
    .ForMember(d => d.VendorName,
        o => o.MapFrom(s => s.Vendor.Name));


       CreateMap<ProductImage, ProductImageDTO>()
    .ForMember(d => d.ImageId, o => o.MapFrom(s => s.ProductImageId))
    .ForMember(d => d.Url, o => o.MapFrom(s => s.ImageUrl));

        CreateMap<ProductReview, ProductReviewDTO>();
        CreateMap<ProductVariant, ProductVariantDTO>();

        // =======================
        // VENDOR / CATEGORY
        // =======================
        CreateMap<Vendor, VendorDTO>();
        CreateMap<Category, CategoryDTO>();

        // =======================
        // CUSTOMER
        // =======================
        CreateMap<CustomerProfile, CustomerDTO>();

CreateMap<UpdateCustomerProfileDTO, CustomerProfile>()
    .ForAllMembers(opts =>
        opts.Condition((src, dest, srcMember) => srcMember != null));


        // =======================
        // CART
        // =======================
        CreateMap<Cart, CartDTO>()
            .ForMember(d => d.Items,
                o => o.MapFrom(s => s.CartItems));

        CreateMap<CartItem, CartItemDTO>()
            .ForMember(d => d.ProductName,
                o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.UnitPrice,
                o => o.MapFrom(s => s.ProductVariant.Price))
            .ForMember(d => d.Images,
                o => o.MapFrom(s =>
                    s.Product.Images.Select(i => i.ImageUrl)));

        // =======================
        // ORDERS
        // =======================
        CreateMap<Order, OrderDTO>();
        CreateMap<OrderItem, OrderItemDTO>();
        CreateMap<OrderAddress, DeliveryAddressDTO>();
        CreateMap<DeliveryAddress, DeliveryAddressDTO>();

        // =======================
        // PAYMENTS
        // =======================
        CreateMap<Invoice, InvoiceDTO>()
            .ForMember(d => d.Id,
                o => o.MapFrom(s => s.InvoiceId));

        CreateMap<Payment, PaymentDTO>();
        CreateMap<CreatePaymentDto, Payment>()
            .ForMember(d => d.PaymentId, o => o.Ignore())
            .ForMember(d => d.Status, o => o.MapFrom(_ => "pending"))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

        // =======================
        // UI / HOME
        // =======================
        CreateMap<Banner, BannerDTO>();
        CreateMap<HomeSection, HomeSectionDTO>();
        CreateMap<HomeAd, HomeAdDTO>();
        CreateMap<HomeSectionProduct, HomeSectionProductDTO>();

        // =======================
        // OTHERS
        // =======================
        CreateMap<Favourites, FavouritesDTO>();
        CreateMap<InventoryLog, InventoryLogDTO>();
        CreateMap<GuestUser, GuestUserDTO>();
        CreateMap<Referral, ReferralDTO>();
        CreateMap<ReferralCode, ReferralCodeDTO>()
            .ForMember(d => d.DiscountType,
                o => o.MapFrom(s => s.DiscountType.ToString()));

    
    
    }
}
