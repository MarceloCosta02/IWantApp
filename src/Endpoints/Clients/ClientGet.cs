using IWantApp.Context;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public static class ClientGet
{
    public static string Template => "/client";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(HttpContext httpContext)
    {
        var user = httpContext.User;

        var result = new
        {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Name = user.Claims.First(c => string.Equals(c.Type, "name", StringComparison.OrdinalIgnoreCase)).Value,
            Cpf = user.Claims.First(c => string.Equals(c.Type, "cpf", StringComparison.OrdinalIgnoreCase)).Value
        };

        return Results.Ok(result);
    }
}
