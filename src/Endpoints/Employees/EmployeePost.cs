using IWantApp.Domain.Request;
using IWantApp.Domain.Users;

namespace IWantApp.Endpoints.Employees;

public static class EmployeePost
{
    public static string Template => "/employee";

    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeeMasterPolicy")]
    public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext httpContext, UserCreator userCreator)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            new Claim("CreatedBy", userId)
        };

        (IdentityResult identity, string userId) result = await userCreator.Create(employeeRequest.Email, employeeRequest.Password, userClaims);

        if (!result.identity.Succeeded)
            return Results.BadRequest(result.identity.Errors.FirstOrDefault());                 

        return Results.Created($"/employee/{ result.userId }", result.userId);
    }
}