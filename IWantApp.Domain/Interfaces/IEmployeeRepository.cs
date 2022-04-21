using IWantApp.Domain.Response;

namespace IWantApp.Domain.Interfaces;

public interface IEmployeeRepository
{
    IEnumerable<EmployeeResponse> QueryAllUsersWithClaimName(int page, int rows);
}