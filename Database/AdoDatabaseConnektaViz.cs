using Microsoft.Data.SqlClient;

namespace ConnektaViz.API.Database;

public interface IAdoDatabaseConnektaViz
{
    public Task ExecuteNonQueryAsync(string query);
    public Task<string> ExecuteScalarValueAsync(string query);
}

public class AdoDatabaseConnektaViz(IConfiguration configuration) : IAdoDatabaseConnektaViz
{
    public async Task ExecuteNonQueryAsync(string query)
    {
        try
        {
            using var connection = new SqlConnection(configuration["ConnectionStrings:InsightX"]);
            using var cmd = new SqlCommand(query, connection);
            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> ExecuteScalarValueAsync(string query)
    {
        try
        {
            string result = string.Empty;
            using var connection = new SqlConnection(configuration["ConnectionStrings:InsightX"]);
            using var cmd = new SqlCommand(query, connection);
            await connection.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                reader.Read();
                result = reader.GetString(0);
            }
            await connection.CloseAsync();

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
