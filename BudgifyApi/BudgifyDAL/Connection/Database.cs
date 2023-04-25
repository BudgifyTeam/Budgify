using BudgifyModels;
using Npgsql;

namespace BudgifyDal.Connection;

public class Database
{
    private readonly string _connectString =
        "Server=144.22.175.160;Port=5432;Database=BudgifyDbTest;User Id=postgres;Password=budgifyPASS";

    public async Task<Response<String>> Connect()
    {
        Response<String> response = new Response<string>();
        try
        {
            await using var conn = new NpgsqlConnection(_connectString);
            await conn.OpenAsync();
        }
        catch (NpgsqlException e) {
            response.message = ("There was an error connecting to the database, error: "+e);
            
        }
        return response;
    }
}