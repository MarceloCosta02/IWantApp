using Dapper;
using IWantApp.Domain.Interfaces;
using IWantApp.Domain.Response;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace IWantApp.Infra.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration _configuration;

        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<EmployeeResponse>> QueryAllUsersWithClaimNameAsync(int page, int rows)
        {
            var db = new SqlConnection(_configuration["ConnectionString:IWantDb"]);

            var query =
                @"SELECT Email, ClaimValue as Name
                FROM AspNetUsers u
                INNER JOIN AspNetUserClaims c
                ON u.Id = c.UserId AND c.ClaimType = 'Name'
              ORDER BY Name
              OFFSET (@page -1 ) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

            return await db.QueryAsync<EmployeeResponse>(query, new { page, rows });
        }       
    }
}
