using IWantApp.Domain.Models.Products;

namespace IWantApp.Domain.Response;

public record OrderResponse(Guid orderId, string Email, IEnumerable<ProductOrderResponse> Products, decimal Total, string DeliveryAddress);