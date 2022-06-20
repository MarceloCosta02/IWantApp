using IWantApp.Domain.Interfaces;

namespace IWantApp.Endpoints.Employees;

public static class EmployeeGetAll
{
    public static string Template => "/employees";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(int? page, int? rows, IEmployeeRepository employeeRepository)
    {
        if (page == null || rows == null)
            return Results.BadRequest("Favor informar a página e a quantidade de linhas");

        var result = await employeeRepository.QueryAllUsersWithClaimNameAsync(page.Value, rows.Value);

        return Results.Ok(result);
    }
}