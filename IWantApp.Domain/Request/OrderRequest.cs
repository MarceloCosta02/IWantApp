namespace IWantApp.Domain.Request;

public record OrderRequest(List<Guid> ProductIds, string DeliveryAddress);