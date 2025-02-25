using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace eCommerce.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresConnection"));
    }

    public IDbConnection DbConnection => _connection;
}
