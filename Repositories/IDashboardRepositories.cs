namespace ConnektaViz.API.Repositories;

public interface IDashboardRepository
{
    Task<ResponseDto> GetAllAsync();
    Task<ResponseDto> GetByIdAsync(long id);
    Task<ResponseDto> SaveAsync(DashboardRequestDto requestDto);
    Task<ResponseDto> DeleteAsync(int id);
    Task<ResponseDto> UpdateSortOrderAsync(List<DashboardRequestDto> requestDto);
}

public class DashboardRepository(ConnektaVizContext dbContext, IMapper mapper, ILogger<DashboardRepository> logger)
    : IDashboardRepository
{
    public async Task<ResponseDto> GetAllAsync()
    {
        try
        {
            var entities = await dbContext.Dashboard.OrderBy(o => o.SortOrder).ToListAsync();
            var result = mapper.Map<IEnumerable<DashboardResponseDto>>(entities);
            return new ResponseDto
            {
                Data = result,
                Message = "Request successfully executed.",
                Success = true,
                Total = result.LongCount()
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving all Dashboard.");
            throw;
        }
    }
    public async Task<ResponseDto> GetByIdAsync(long id)
    {
        var response = new ResponseDto
        {
            Message = "Request completed successfully",
            Success = true,
            Total = 1
        };

        try
        {
            var entity = await dbContext.Dashboard
                        .Include(d => d.DashboardGraphs)
                            .ThenInclude(a => a.Graph)
                                .ThenInclude(z => z.GraphType)
                        .Include(d => d.DashboardGraphs)
                            .ThenInclude(a => a.Graph)
                                .ThenInclude(b => b.GraphTableFilters)
                        .Include(d => d.DashboardGraphs)
                            .ThenInclude(a => a.Graph)
                                .ThenInclude(b => b.GraphColumns)
                        .Include(d => d.DashboardGraphs)
                            .ThenInclude(a => a.Graph)
                                .ThenInclude(b => b.GraphStyling)
                        .FirstOrDefaultAsync(d => d.Id == id);

            response.Data = mapper.Map<DashboardResponseDto>(entity);
            return response;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return response;
        }
    }
    public async Task<ResponseDto> SaveAsync(DashboardRequestDto requestDto)
    {
        var response = new ResponseDto
        {
            Data = null,
            Message = "Record save successfully.",
            Success = true,
            Total = 1
        };

        try
        {
            if (requestDto.Id > 0)
            {
                var entity = await dbContext.Dashboard.Include(x => x.DashboardGraphs).FirstOrDefaultAsync(x => x.Id.Equals(requestDto.Id));
                if (entity is null)
                {
                    response.Message = "Record not found";
                    response.Success = false;
                    return response;
                }
                mapper.Map<DashboardRequestDto, Dashboard>(requestDto, entity);
            }
            else
            {
                var dashboard = mapper.Map<Dashboard>(requestDto);

                dashboard.SortOrder = await dbContext.Dashboard.AnyAsync() ? await dbContext.Dashboard.MaxAsync(x => x.SortOrder) + 1 : 1;
                dbContext.Dashboard.Add(dashboard);
            }

            await dbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while saving the dashboard.");
            response.Message = ex.Message;
            response.Success = false;
        }
        return response;
    }

    public async Task<ResponseDto> UpdateSortOrderAsync(List<DashboardRequestDto> requestDto)
    {
        var response = new ResponseDto
        {
            Data = null,
            Message = "Record save successfully.",
            Success = true,
            Total = 1
        };

        try
        {
            var requestedIds = requestDto.Select(s => s.Id);
            var entities = await dbContext.Dashboard
                .Where(x => requestedIds.Contains(x.Id))
                .ToListAsync();

            if (!entities.HasAny())
            {
                response.Message = "No records found.";
                response.Success = false;
                return response;
            }

            var entityDictionary = entities.ToDictionary(e => e.Id);

            for (int index = 0; index < requestDto.Count; index++)
            {
                var item = requestDto[index];
                if (entityDictionary.TryGetValue(item.Id, out var entity))
                {
                    entity.SortOrder = index;
                }
            }

            dbContext.Dashboard.UpdateRange(entities);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while saving the dashboard.");
            response.Message = ex.Message;
            response.Success = false;
        }
        return response;
    }
    public async Task<ResponseDto> DeleteAsync(int id)
    {
        var response = new ResponseDto
        {
            Data = null,
            Message = "Record delete successfully.",
            Success = true,
            Total = 1
        };
        try
        {
            var entity = dbContext.Dashboard.Include(i => i.DashboardGraphs);
            var dashboard = await entity.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (dashboard == null)
            {
                response.Success = false;
                response.Message = "Record not found.";
                return response;
            }

            dbContext.DashboardGraph.RemoveRange(dashboard.DashboardGraphs);
            dbContext.Dashboard.Remove(dashboard);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
