namespace ConnektaViz.API.DTOs.Request
{
    public class DashboardRequestDto
    {
        public DashboardRequestDto()
        {
            DashboardGraphs = new List<DashboardGraphRequestDto>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<DashboardGraphRequestDto> DashboardGraphs { get; set; }
    }
}