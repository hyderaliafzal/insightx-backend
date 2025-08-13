using System.Data.Common;

namespace ConnektaViz.API.Repositories;

public interface IDataRepository
{
    Task<ResponseDto> GetSourceNames();
    Task<ResponseDto> GetPropertiesAsync(string tableName);
    Task<ResponseDto> GetDataForGraph(GraphQueryDto request);
    Task<ResponseDto> GetDataForTable(QueryDto request);
    Task<ResponseDto> GetMatric(MatricRequestDto request);
    Task<string> GetDataAsCsv(FileExportRequestDto requestDto);
}
public class DataRepository(ConnektaVizContext dbContext, IMapper mapper, IAdoDatabaseConnektaViz adoDatabase, IWebHostEnvironment environment) : IDataRepository
{
    public async Task<ResponseDto> GetSourceNames()
    {
        var query = await dbContext.Table.ToListAsync();
        var result = mapper.Map<IEnumerable<TableResponseDto>>(query);

        return new ResponseDto
        {
            Data = result,
            Total = result.LongCount(),
            Success = true,
            Message = "Record fetch successfully."
        };
    }

    public async Task<ResponseDto> GetPropertiesAsync(string table)
    {
        var query = await dbContext.Column.FromSqlInterpolated($"EXEC SP_GetColumns @table = {table}").ToListAsync();
        var result = mapper.Map<IEnumerable<ColumnResponseDto>>(query);

        return new ResponseDto
        {
            Data = result,
            Total = result.LongCount(),
            Success = true,
            Message = "Record fetch successfully."
        };
    }

    public async Task<ResponseDto> GetDataForTable(QueryDto request)
    {
        using var connection = dbContext.Database.GetDbConnection();

        using var command = connection.CreateCommand();
        command.CommandText = "GetData ";
        command.CommandType = CommandType.StoredProcedure;

        var tableNameParam = command.CreateParameter();
        tableNameParam.ParameterName = "@TableName";
        tableNameParam.Value = request.DataSource;
        command.Parameters.Add(tableNameParam);

        var skipParam = command.CreateParameter();
        skipParam.ParameterName = "@Skip";
        skipParam.Value = request.PageNumber * request.PageSize;
        command.Parameters.Add(skipParam);

        var takeParam = command.CreateParameter();
        takeParam.ParameterName = "@Take";
        takeParam.Value = request.PageSize;
        command.Parameters.Add(takeParam);

        var sortParam = command.CreateParameter();
        sortParam.ParameterName = "@Sort";
        sortParam.Value = request.Sort;
        command.Parameters.Add(sortParam);

        var sortOrderParam = command.CreateParameter();
        sortOrderParam.ParameterName = "@SortOrder";
        sortOrderParam.Value = !string.IsNullOrWhiteSpace(request.SortOrder) && request.SortOrder.Equals("descend") ? "DESC" : null;
        command.Parameters.Add(sortOrderParam);

        string filter = GetFilters(request.Filters);
        var filterParam = command.CreateParameter();
        filterParam.ParameterName = "@filter";
        filterParam.Value = filter;
        command.Parameters.Add(filterParam);

        var data = new List<Dictionary<string, object>>();

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);

            data.Add(row);
        }
        await connection.CloseAsync();

        long total = await GetTotalCount(dbContext, request.DataSource, filter);
        return new ResponseDto { Data = data, Total = total, Success = true, Message = "Request process successfully." };

    }

    public async Task<ResponseDto> GetDataForGraph(GraphQueryDto request)
    {
        var result = new List<KeyValue>();

        if (string.IsNullOrEmpty(request.XColumn))
            return new ResponseDto { Data = result, Total = 0, Success = false, Message = "XColumn must be provided." };

        if (!string.IsNullOrEmpty(request.YColumn))
            result.AddRange(await GetQuantativeData(request));
        else
            result.AddRange(await GetCategoricalData(request));

        return new ResponseDto { Data = result, Total = result.Count, Success = true, Message = "Records fetched successfully." };
    }

    public async Task<ResponseDto> GetMatric(MatricRequestDto request)
    {
        string query = $"Select dbo.fn_FormatNumber({request.MatricFunction}([{request.BindingColumn}])) AS Val FROM [{request.DataSource}]";
        query = request.Filters.HasAny() ? string.Concat(query, $" {GetFilters(request.Filters)}") : query;
        return new ResponseDto
        {
            Data = await adoDatabase.ExecuteScalarValueAsync(query),
            Success = true,
            Message = "Request execute successfully.",
            Total = 0
        };
    }

    public async Task<string> GetDataAsCsv(FileExportRequestDto requestDto)
    {
        DbConnection connection = dbContext.Database.GetDbConnection();
        DbCommand command = connection.CreateCommand();

        var query = @$"SELECT * FROM [{requestDto.DataSource}] {GetFilters(requestDto.Filters)}";
        command.CommandText = query;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        string filename = $"{DateTime.UtcNow:ddMMyyyyHHmmssfffffff}.csv";
        using var writer = new StreamWriter(string.Concat(environment.WebRootPath, "/", filename));

        string[] columnNames = GetColumnNames(reader);
        writer.WriteLine(string.Join(",", columnNames));

        while (reader.Read())
        {
            string[] row = new string[reader.FieldCount];

            for (int i = 0; i < reader.FieldCount; i++)
                row[i] = reader[i].ToString();

            writer.WriteLine(string.Join(",", row));
        }
        await connection.CloseAsync();
        return filename;
    }

    static async Task<long> GetTotalCount(ConnektaVizContext dbContext, string dataSource, string filter)
    {
        DbConnection connection = dbContext.Database.GetDbConnection();
        DbCommand countCommand = connection.CreateCommand();

        await connection.OpenAsync();
        var queryCount = @$"SELECT ISNULL(CONVERT(bigint,COUNT(*)),0) AS Total FROM [{dataSource}] {filter}";

        countCommand.CommandText = queryCount;
        long total = (long)await countCommand.ExecuteScalarAsync();

        await connection.CloseAsync();
        return total;
    }

    string GetFilters(IEnumerable<FilterDto> filters)
    {
        if (!filters.HasAny()) return string.Empty;

        string where = "WHERE (1=1) AND {0}";
        var conditons = filters.Select(s => $"[{s.Name}] {s.Operation} '{s.Keyword}'");
        return string.Format(where, string.Join(" AND ", conditons));
    }

    async Task<List<KeyValue>> GetCategoricalData(GraphQueryDto request)
    {
        string query = $"SELECT CONVERT(nvarchar(max),[{request.XColumn}]) AS [Label], CONVERT(nvarchar(max),ISNULL({request.MatricFunction}(*), '0')) AS [Value] " +
                       $"FROM [{request.DataSource}] " +
                       $" {GetFilters(request.Filters)} " +
                       $"GROUP BY [{request.XColumn}]";

        return await ExecuteQuery(query);
    }

    async Task<List<KeyValue>> GetQuantativeData(GraphQueryDto request)
    {
    string query = $"SELECT CONVERT(nvarchar(max),[{request.XColumn}]) AS Label, CONVERT(nvarchar(max),ISNULL([{request.YColumn}],'0')) AS [Value] " +
                   $"FROM [{request.DataSource}] {GetFilters(request.Filters)}";

        if (!string.IsNullOrEmpty(request.MatricFunction))
        {
            query = $"SELECT CONVERT(nvarchar(max),[{request.XColumn}]) AS Label, CONVERT(nvarchar(max),{request.MatricFunction}(ISNULL([{request.YColumn}],'0'))) AS [Value] " +
                    $"FROM [{request.DataSource}] {GetFilters(request.Filters)}" +
                    $"GROUP BY {request.XColumn}";
        }

        return await ExecuteQuery(query);
    }

    async Task<List<KeyValue>> ExecuteQuery(string query) => await dbContext.KeyValue.FromSqlRaw(query).ToListAsync();

    private static string[] GetColumnNames(DbDataReader reader)
    {
        string[] columnNames = new string[reader.FieldCount];
        for (int i = 0; i < reader.FieldCount; i++)
        {
            columnNames[i] = reader.GetName(i);
        }
        return columnNames;
    }
}
