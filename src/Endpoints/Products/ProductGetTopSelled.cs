using IWantApp.Context;
using IWantApp.Domain.Interfaces;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;

namespace IWantApp.Endpoints.Products;

public static class ProductGetTopSelled
{
    public static string Template => "/products/top";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context, IProductRepository productRepository)
    {
        var result = await productRepository.QueryTopProductsSelledAsync();

        if (!result.Any())
            throw new Exception("Erro ao consultar produtos mais vendidos");

        return Results.Ok(result.Select(p => new TopProductsResponse(p.IdProduto, p.Name, p.Total)));
    }
}
