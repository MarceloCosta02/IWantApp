using IWantApp.Domain.Response;

namespace IWantApp.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<TopProductsResponse>> QueryTopProductsSelledAsync();
}