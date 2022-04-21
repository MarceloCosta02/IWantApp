using Dapper;
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

    public static IResult Action(int? page, int? rows, IConfiguration configuration)
    {
        if (page == null || rows == null)
            return Results.BadRequest("Favor informar a página e a quantidade de linhas");

        var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);

        var query = 
            @"SELECT Email, ClaimValue as Name
                FROM AspNetUsers u
                INNER JOIN AspNetUserClaims c
                ON u.Id = c.UserId AND c.ClaimType = 'Name'
              ORDER BY c.ClaimType 
              OFFSET (@page -1 ) * @rows FETCH NEXT @rows ROWS ONLY";

        var employees = db.Query<EmployeeResponse>(query, new { page, rows });

        return Results.Ok(employees);
    }
}