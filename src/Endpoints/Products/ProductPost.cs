using IWantApp.Context;
using IWantApp.Domain.Models.Products;
using IWantApp.Domain.Request;

namespace IWantApp.Endpoints.Categories;

public static class ProductPost
{
    public static string Template => "/products";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRequest productRequest, HttpContext httpContext, ApplicationDbContext context)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == productRequest.CategoryId);
        var product = new Product(productRequest.Name, category, productRequest.Description, productRequest.HasStock, productRequest.Price, userId);

        if (!product.IsValid)
            return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return Results.Created($"/products/{product.Id}", product.Id);
    }
}
