using IWantApp.Context;
using IWantApp.Domain.Models.Orders;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace IWantApp.Endpoints.Categories;

public static class OrderGetByAuth
{
    public static string Template => "/orders/{cpf}";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "OrderPolicy")]
    public static async Task<IResult> Action(HttpContext httpContext, ApplicationDbContext context, UserManager<IdentityUser> userManager, string cpf)
    {
        var claims = httpContext.User.Claims;

        var userId = context.UserClaims
            .Where(c => c.ClaimValue.Contains(cpf))
            .FirstOrDefault().UserId;

        (bool clientValid, string id) client = IsClient(claims);

        if ((client.clientValid && client.id == userId) || IsEmployee(claims))
        {
            var orders = context
              .Orders
              .AsNoTracking()
              .Include(p => p.Products)
              .Where(p => p.ClientId == userId)
              .ToList();
;
            if (orders == null)
                return Results.BadRequest("Não existem ordens para o cliente informado");

            var user = await userManager.FindByIdAsync(userId);
            var ordersWithProductsNormalized = NormalizarProdutosDaOrdem(orders, user.Email);            

            return Results.Ok(ordersWithProductsNormalized);
        }
        else
            return Results.Forbid();
    }

    private static IEnumerable<OrderResponse> NormalizarProdutosDaOrdem(List<Order> orders, string email)
    {
        return (from o in orders
                let product = o.Products.ConvertAll(p => new ProductOrderResponse(p.Name, p.Description, p.Price))    
                select new OrderResponse(o.Id, email, product, o.Total, o.DeliveryAddress)).ToList();
    }

    private static bool IsEmployee(IEnumerable<Claim> claim)
    {
        var employee = claim.FirstOrDefault(c => c.Type == "EmployeeCode");
        return employee != null;
    }

    private static (bool, string) IsClient(IEnumerable<Claim> claim)
    {
        var client = claim.FirstOrDefault(c => c.Type == "Cpf");
        if (client != null)
        {
            string id = claim.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return (true, id);
        }

        return (false, string.Empty);
    }
}
