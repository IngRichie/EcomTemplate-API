using AutoMapper;
using EcomTemplate.Application.DTOs;
using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Mapping;

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
            .ReverseMap()
            .ForMember(d => d.Category, o => o.Ignore()); // 🔥 prevent navigation issues

        // =======================
        // PRODUCT IMAGES
        // =======================

        CreateMap<ProductImage, ProductImageDTO>()
            .ForMember(d => d.ImageId,
                o => o.MapFrom(s => s.ProductImageId))
            .ForMember(d => d.Url,
                o => o.MapFrom(s => s.ImageUrl))
            .ReverseMap()
            .ForMember(d => d.ProductImageId,
                o => o.MapFrom(s => s.ImageId))
            .ForMember(d => d.ImageUrl,
                o => o.MapFrom(s => s.Url))
            .ForMember(d => d.Product, o => o.Ignore());

        // =======================
        // PRODUCT VARIANTS
        // =======================

        CreateMap<ProductVariant, ProductVariantDTO>()
            .ReverseMap()
            .ForMember(d => d.Product, o => o.Ignore());

        // =======================
        // VARIANT ATTRIBUTES
        // =======================

        CreateMap<ProductVariantAttribute, ProductVariantAttributeDTO>()
            .ReverseMap()
            .ForMember(d => d.ProductVariant, o => o.Ignore());

        // =======================
        // REVIEWS
        // =======================

        CreateMap<ProductReview, ProductReviewDTO>()
            .ReverseMap()
            .ForMember(d => d.Product, o => o.Ignore());

        // =======================
        // CATEGORY
        // =======================

        CreateMap<Category, CategoryDTO>()
            .ReverseMap();

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
        CreateMap<HomeSectionProduct, HomeSectionProductDTO>();

        // =======================
        // OTHERS
        // =======================

        CreateMap<Favourites, FavouritesDTO>();
    }
}