using IWantApp.Domain.Response;

namespace IWantApp.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeResponse>> QueryAllUsersWithClaimNameAsync(int page, int rows);
}