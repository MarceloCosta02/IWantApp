using Dapper;
using IWantApp.Domain.Interfaces;
using IWantApp.Domain.Request;
using IWantApp.Domain.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public static class EmployeeGetAll
{
    public static string Template => "/employees";

    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, IEmployeeRepository employeeRepository)
    {
        if (page == null || rows == null)
            return Results.BadRequest("Favor informar a página e a quantidade de linhas");       

        return Results.Ok(employeeRepository.QueryAllUsersWithClaimName(page.Value, rows.Value));
    }
}