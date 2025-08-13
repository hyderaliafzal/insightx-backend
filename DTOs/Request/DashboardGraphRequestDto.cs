namespace ConnektaViz.API.DTOs.Request
{
    public class DashboardGraphRequestDto
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int DashboardId { get; set; }
        public int GraphId { get; set; }
        public GraphResponseDto Graph { get; set; }
    }
}