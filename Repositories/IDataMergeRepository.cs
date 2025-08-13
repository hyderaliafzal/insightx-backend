namespace ConnektaViz.API.Repositories;

public interface IDataMergeRepository
{
    Task<ResponseDto> Delete(int id);
    Task<ResponseDto> GetAsync(RequestDto request);
    Task<bool> IsDuplicate(MergeQueryRequestDto request);
    Task<ResponseDto> SaveAsync(MergeQueryRequestDto request);
}
public class DataMergeRepository(ConnektaVizContext dbContext, IMapper mapper, IAdoDatabaseConnektaViz adoDatabase, IDataRepository dataRepository) : IDataMergeRepository
{
    public async Task<ResponseDto> GetAsync(RequestDto request)
    {
        var response = new ResponseDto
        {
            Success = true,
            Message = "Request processed successfully."
        };

        var query = dbContext.MergeQuery.Include(i => i.MergeQueryDetails).Where(w => request.Id.HasValue
        ? w.Id.Equals(request.Id)
        : string.IsNullOrWhiteSpace(request.Search) || w.Name.ToLower().Contains(request.Search.ToLower()));

        query = await query.ApplyPaginationAsync(request.PageNumber, request.PageSize, response);
        response.Data = mapper.Map<IEnumerable<MergeQueryResponseDto>>(query);
        return response;
    }

    public async Task<ResponseDto> SaveAsync(MergeQueryRequestDto request)
    {
        var response = new ResponseDto { Success = true, Message = "Record save successfully." };
        try
        {
            if (request.Id > 0)
            {
                var entity = await dbContext.MergeQuery.Include(i => i.MergeQueryDetails).FirstOrDefaultAsync(f => f.Id.Equals(request.Id));
                if (entity is null)
                {
                    response.Message = "Record not found";
                    response.Success = false;
                    return response;
                }
                mapper.Map<MergeQueryRequestDto, MergeQuery>(request, entity);
            }
            else
            {
                var entity = mapper.Map<MergeQuery>(request);

                dbContext.Add(entity);
            }
            await dbContext.SaveChangesAsync();

            await adoDatabase.ExecuteNonQueryAsync($"DROP VIEW IF EXISTS [{request.Name}]");
            string query = "CREATE VIEW [{0}] AS SELECT {1} FROM {2}";
            string joinQuery = "";
            string selectStatement = "";
            var common = new List<string>();
            for (int i = 0; i < request.MergeQueryDetails.Count(); i++)
            {
                var obj = request.MergeQueryDetails[i];
                if (i > 0)
                {
                    var rightTableColumns = await dataRepository.GetPropertiesAsync(obj.RightTable);
                    var rightTableResult = rightTableColumns.Data as IEnumerable<ColumnResponseDto>;

                    if (common.HasAny())
                        rightTableResult = rightTableResult.Where(w => !common.Contains(w.Name));

                    selectStatement = string.Concat(selectStatement, ",", string.Join(",", rightTableResult.Select(s => $"{obj.RightTableAlias}.[{s.Name}]")));

                    joinQuery += $"{obj.JoinType.ToUpper()}  " +
                            $"[{obj.RightTable}] AS {obj.RightTableAlias}  " +
                            $"ON " +
                            $"{obj.LeftTableAlias}.[{obj.PrimaryColumn}] " +
                            $"{obj.Operator} " +
                            $"{obj.RightTableAlias}.[{obj.ForeignColumn}] ";
                }
                else
                {
                    var leftTableColumns = await dataRepository.GetPropertiesAsync(obj.LeftTable);
                    var rightTableColumns = await dataRepository.GetPropertiesAsync(obj.RightTable);

                    var leftTableResult = leftTableColumns.Data as IEnumerable<ColumnResponseDto>;
                    var rightTableResult = rightTableColumns.Data as IEnumerable<ColumnResponseDto>;

                    common = leftTableResult.Select(s => s.Name).Intersect(rightTableResult.Select(s => s.Name)).ToList();
                    string firstSelectStatement = string.Join(",", leftTableResult.Select(s => $"{obj.LeftTableAlias}.[{s.Name}]"));
                    selectStatement = string.Join(",", rightTableResult.Where(w => !common.Contains(w.Name)).Select(s => $"{obj.RightTableAlias}.[{s.Name}]"));

                    selectStatement = string.Concat(firstSelectStatement, ",", selectStatement);


                    joinQuery += $"[{obj.LeftTable}] AS {obj.LeftTableAlias} " +
                            $"{obj.JoinType.ToUpper()}  " +
                            $"[{obj.RightTable}] AS {obj.RightTableAlias}  " +
                            $"ON " +
                            $"{obj.LeftTableAlias}.[{obj.PrimaryColumn}] " +
                            $"{obj.Operator} " +
                            $"{obj.RightTableAlias}.[{obj.ForeignColumn}] ";
                }
            }

            await adoDatabase.ExecuteNonQueryAsync(string.Format(query, request.Name, selectStatement, joinQuery));
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
        }
        return response;
    }

    public async Task<ResponseDto> Delete(int id)
    {
        var entity = await dbContext.MergeQuery.Include(i => i.MergeQueryDetails).FirstOrDefaultAsync(f => f.Id.Equals(id));
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();

        await adoDatabase.ExecuteNonQueryAsync($"DROP VIEW IF EXISTS {entity.Name};");
        return new ResponseDto { Success = true, Message = "Record deleted successfully." };
    }

    public async Task<bool> IsDuplicate(MergeQueryRequestDto request)
    {
        return await dbContext.MergeQuery.Where(w => request.Id <= 0 && w.Name.ToLower().Equals(request.Name.ToLower())).AnyAsync();
    }
}
