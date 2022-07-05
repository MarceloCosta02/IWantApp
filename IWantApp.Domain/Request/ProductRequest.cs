namespace IWantApp.Domain.Request;

public record ProductRequest(string Name, Guid CategoryId, string Description, bool HasStock, bool Active);