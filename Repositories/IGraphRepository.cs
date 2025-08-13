namespace ConnektaViz.API.Repositories;

public interface IGraphRepository
{
    ResponseDto GetAll();
    Task<ResponseDto> GetById(long id);
    ResponseDto GetGraphTypes();
    Task<ResponseDto> DeleteAsync(long id);
    Task<ResponseDto> SaveAsync(GraphRequestDto requestDto);
    Task<bool> IsExists(Expression<Func<Graph, bool>> expression);
}

public class GraphRepository(ConnektaVizContext dbContext, IMapper mapper) : IGraphRepository
{
    public ResponseDto GetAll()
    {
        var query = dbContext.Graph
            .Include(_ => _.GraphType)
            .Include(_ => _.GraphTableFilters);
        var result = mapper.Map<IEnumerable<GraphResponseDto>>(query);

        return new ResponseDto
        {
            Data = result,
            Total = result.LongCount(),
            Success = true,
            Message = "Record fetch successfully."
        };
    }

    public async Task<ResponseDto> GetById(long id)
    {
        var query = await dbContext.Graph
            .Include(_ => _.GraphType)
            .Include(_ => _.GraphColumns)
            .Include(_ => _.SelectedTableColumns)
            .Include(_ => _.GraphTableFilters)
            .Include(_ => _.GraphStyling)
            .FirstOrDefaultAsync(f => f.Id.Equals(id));

        var result = mapper.Map<GraphResponseDto>(query);
        return new ResponseDto
        {
            Data = result,
            Total = 1,
            Success = true,
            Message = "Record fetch successfully."
        };
    }

    public ResponseDto GetGraphTypes()
    {
        var result = mapper.Map<IEnumerable<GraphTypeResponseDto>>(dbContext.GraphType.Where(_ => _.IsActive));
        return new ResponseDto
        {
            Data = result,
            Total = result.LongCount(),
            Success = true,
            Message = "Record fetch successfully."
        };
    }

    public async Task<ResponseDto> DeleteAsync(long id)
    {
        var result = await dbContext.Graph
            .Include(i => i.GraphColumns)
            .Include(i => i.SelectedTableColumns)
            .Include(i => i.GraphTableFilters)
            .Include(i => i.GraphStyling)
            .FirstOrDefaultAsync(f => f.Id.Equals(id));

        if (result.GraphColumns.HasAny())
            dbContext.RemoveRange(result.GraphColumns);

        if (result.SelectedTableColumns.HasAny())
            dbContext.RemoveRange(result.SelectedTableColumns);

        if (result.GraphTableFilters.HasAny())
            dbContext.RemoveRange(result.GraphTableFilters);

        if (result.GraphStyling.HasAny())
            dbContext.RemoveRange(result.GraphStyling);

        dbContext.Remove(result);
        await dbContext.SaveChangesAsync();

        return new ResponseDto
        {
            Data = result,
            Total = 1,
            Success = true,
            Message = "Record deleted successfully."
        };
    }

    public async Task<bool> IsExists(Expression<Func<Graph, bool>> expression) => await dbContext.Graph.AnyAsync(expression);

    public async Task<ResponseDto> SaveAsync(GraphRequestDto requestDto)
    {
        try
        {
            if (requestDto.Id > 0)
            {
                var existingGraph = await dbContext.Graph
                    .Include(i => i.GraphColumns)
                    .Include(i => i.SelectedTableColumns)
                    .Include(i => i.GraphTableFilters)
                    .Include(i=>i.GraphStyling)
                    .FirstOrDefaultAsync(f => f.Id.Equals(requestDto.Id));
                Console.WriteLine(existingGraph);
                mapper.Map<GraphRequestDto, Graph>(requestDto, existingGraph);
                await dbContext.GraphColumn.AddRangeAsync(existingGraph.GraphColumns.Where(w => w.Id.Equals(0)));

                dbContext.Update(existingGraph);
            }
            else
            {
                var entity = mapper.Map<Graph>(requestDto);
                dbContext.Add(entity);
            }

            await dbContext.SaveChangesAsync();
            return new ResponseDto
            {
                Data = null,
                Total = 1,
                Success = true,
                Message = "Record save successfully."
            };
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
