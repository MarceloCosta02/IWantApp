using IWantApp.Context;
using IWantApp.Domain.Models.Orders;
using IWantApp.Domain.Models.Products;
using IWantApp.Domain.Request;
using IWantApp.Domain.Users;

namespace IWantApp.Endpoints.Clients;

public static class OrderPost
{
    public static string Template => "/orders";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext httpContext, ApplicationDbContext context)
    {
        var clientId = httpContext.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;      

        var clientName = httpContext.User.Claims
            .First(c => string.Equals(c.Type, "name", StringComparison.OrdinalIgnoreCase)).Value;

        List<Product> productsFound = null;

        if (orderRequest.ProductIds != null || orderRequest.ProductIds.Any())
        {
            // Busca no banco de dados todos os produtos que possuam os productIds da requisição
            productsFound = context.Products.Where(p => orderRequest.ProductIds.Contains(p.Id)).ToList();
        }

        var order = new Order(clientId, clientName, productsFound, orderRequest.DeliveryAddress);

        if (!order.IsValid)
            return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());       

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}