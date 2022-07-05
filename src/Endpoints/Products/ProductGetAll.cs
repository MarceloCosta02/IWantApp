using IWantApp.Context;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;

namespace IWantApp.Endpoints.Categories;

public static class ProductGetAll
{
    public static string Template => "/products";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();

        if (products == null)
            return Results.BadRequest("Não existem produtos cadastrados");

        var results = products.Select(p => new ProductResponse(p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Active));

        return Results.Ok(results);
    }
}
