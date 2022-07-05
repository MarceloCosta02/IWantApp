using IWantApp.Context;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public static class ProductGetById
{
    public static string Template => "/product/{id}";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    //[Authorize(Policy = "EmployeePolicy")]    
    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, [FromRoute] Guid id)
    {
        var product = context
            .Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);

        var results = new ProductResponse(product.Id, product.Name, product.Category.Name, product.Description, product.HasStock, product.Active);

        return Results.Ok(results);
    }
}
