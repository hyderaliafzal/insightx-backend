namespace ConnektaViz.API.Entities;

using System.ComponentModel.DataAnnotations;

public class GraphStyling
{
    [Key]
    public int Id { get; set; }

    public string BackgroundColor { get; set; }
    public string BorderAlign { get; set; }
    public string BorderColor { get; set; }
    public int[] BorderDash { get; set; }
    public int BorderDashOffset { get; set; }
    public string BorderJoinStyle { get; set; }
    public int BorderRadius { get; set; }
    public int BorderWidth { get; set; }
    public int Offset { get; set; }
    public string Rotation { get; set; }
    public int Spacing { get; set; }
    public int Weight { get; set; }
    public float BarPercentage { get; set; }
    public int BarThickness { get; set; }
    public int MaxBarThickness { get; set; }
    public int MinBarLength { get; set; }
    public string BorderSkipped { get; set; }
    public float CategoryPercentage { get; set; }
    public string BorderCapStyle { get; set; }
    public int LegendFontSize { get; set; }
    public string LegendFontStyle { get; set; }
    public string LegendFontWeight { get; set; }
    public string LegendFontFamily { get; set; }
    public int TooltipFontSize { get; set; }
    public string TooltipFontStyle { get; set; }
    public string TooltipFontWeight { get; set; }
    public string TooltipFontFamily { get; set; }
    public string HoverBackgroundColor { get; set; }
    public string HoverBorderColor { get; set; }
    public int[] HoverBorderDash { get; set; }
    public int HoverBorderDashOffset { get; set; }
    public string HoverBorderJoinStyle { get; set; }
    public int HoverBorderWidth { get; set; }
    public int HoverBorderRadius { get; set; }
    public int HoverOffset { get; set; }
    public string HoverBorderCapStyle { get; set; }
    public string PointBackgroundColor { get; set; }
    public string PointBorderColor { get; set; }
    public int PointBorderWidth { get; set; }
    public int PointRadius { get; set; }
    public int PointHitRadius { get; set; }
    public string PointHoverBackgroundColor { get; set; }
    public string PointHoverBorderColor { get; set; }
    public int PointHoverBorderWidth { get; set; }
    public int PointHoverRadius { get; set; }
    public int PointRotation { get; set; }
    public string PointStyle { get; set; }


    public int Tension { get; set; }
    public bool Fill { get; set; }
    public bool BeginAtZeroX { get; set; }
    public int StepSizeX { get; set; }
    public int MinX { get; set; }
    public int MaxX { get; set; }
    public bool BeginAtZeroY { get; set; }
    public int StepSizeY { get; set; }
    public int MinY { get; set; }
    public int MaxY { get; set; }

    public bool IsXAxisShow { get; set; }
    public bool IsYAxisShow { get; set; }
    public bool IsLegendShow { get; set; }


    [ForeignKey("Graph")]
    public long GraphId { get; set; }
    public virtual Graph Graph { get; set; }
}