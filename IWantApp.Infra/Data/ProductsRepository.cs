using IWantApp.Domain.Interfaces;
using IWantApp.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWantApp.Infra.Data;

public class ProductsRepository : IProductRepository
{
	private readonly IConfiguration _configuration;

	public ProductsRepository(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<IEnumerable<TopProductsResponse>> QueryTopProductsSelledAsync()
	{
		var db = new SqlConnection(_configuration["ConnectionString:IWantDb"]);

		string query =
			@"SELECT p.Id AS IdProduto, p.Name AS Name, count(p.id) AS Total FROM OrderProducts op
				INNER JOIN products p
				ON op.ProductsId = p.Id
				INNER JOIN Orders o
				ON o.Id = op.OrdersId
			GROUP BY p.Name, p.Id
			ORDER BY count(p.Id) DESC";

		var res = await db.QueryAsync<TopProductsResponse>(query);
		return res;
	}
}
