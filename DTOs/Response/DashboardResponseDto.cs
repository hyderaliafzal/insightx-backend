namespace ConnektaViz.API.DTOs.Response
{
    public class DashboardResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DashboardGraphResponseDto> DashboardGraphs { get; set; }
    }
}