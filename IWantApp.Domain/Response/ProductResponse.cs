namespace IWantApp.Domain.Response;

public record ProductResponse(Guid id, string Name, string CategoryName, string Description, bool HasStock, bool Active);