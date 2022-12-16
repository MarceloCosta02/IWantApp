using IWantApp.Context;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;

namespace IWantApp.Endpoints.Categories;

public static class ProductGetShowCase
{
    public static string Template => "/products/showcase";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int row = 1, string orderBy = "name")
    {
        if (row > 10)
            return Results.Problem(title: "Row with max 10", statusCode: 400);

        var queryBase = context.Products.AsNoTracking().Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active); 
        
        switch (orderBy)
        {
            case "name":
                queryBase = queryBase.OrderBy(p => p.Name);
                break;
            case "price":
                queryBase = queryBase.OrderBy(p => p.Price);
                break;
            default:
                return Results.Problem(title: "Order only by price or name", statusCode: 400);
        }

        var queryFilter = queryBase.Skip((page - 1) * row).Take(row);
        
        var products = queryFilter.ToList();

        var results = products.Select(p => new ProductResponse(p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.Active));

        return Results.Ok(results);
    }
}
