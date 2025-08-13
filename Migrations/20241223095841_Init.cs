using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConnektaViz.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Column",
                columns: table => new
                {
                    IsNumber = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FK = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Dashboard",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GraphType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyValue",
                columns: table => new
                {
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MergeQuery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MergeQuery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Graph",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatricFunction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Graph_GraphType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "GraphType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MergeQueryDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MergeQueryId = table.Column<int>(type: "int", nullable: false),
                    LeftTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeftTableAlias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightTableAlias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignColumn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MergeQueryDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MergeQueryDetail_MergeQuery_MergeQueryId",
                        column: x => x.MergeQueryId,
                        principalTable: "MergeQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DashboardGraph",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    DashboardId = table.Column<long>(type: "bigint", nullable: false),
                    GraphId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardGraph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DashboardGraph_Dashboard_DashboardId",
                        column: x => x.DashboardId,
                        principalTable: "Dashboard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DashboardGraph_Graph_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GraphColumn",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraphId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNumber = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphColumn_Graph_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GraphStyling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderAlign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderDash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderDashOffset = table.Column<int>(type: "int", nullable: false),
                    BorderJoinStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderRadius = table.Column<int>(type: "int", nullable: false),
                    BorderWidth = table.Column<int>(type: "int", nullable: false),
                    Offset = table.Column<int>(type: "int", nullable: false),
                    Rotation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Spacing = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    BarPercentage = table.Column<float>(type: "real", nullable: false),
                    BarThickness = table.Column<int>(type: "int", nullable: false),
                    MaxBarThickness = table.Column<int>(type: "int", nullable: false),
                    MinBarLength = table.Column<int>(type: "int", nullable: false),
                    BorderSkipped = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryPercentage = table.Column<float>(type: "real", nullable: false),
                    BorderCapStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegendFontSize = table.Column<int>(type: "int", nullable: false),
                    LegendFontStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegendFontWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegendFontFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TooltipFontSize = table.Column<int>(type: "int", nullable: false),
                    TooltipFontStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TooltipFontWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TooltipFontFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoverBackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoverBorderColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoverBorderDash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoverBorderDashOffset = table.Column<int>(type: "int", nullable: false),
                    HoverBorderJoinStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoverBorderWidth = table.Column<int>(type: "int", nullable: false),
                    HoverBorderRadius = table.Column<int>(type: "int", nullable: false),
                    HoverOffset = table.Column<int>(type: "int", nullable: false),
                    HoverBorderCapStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointBackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointBorderColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointBorderWidth = table.Column<int>(type: "int", nullable: false),
                    PointRadius = table.Column<int>(type: "int", nullable: false),
                    PointHitRadius = table.Column<int>(type: "int", nullable: false),
                    PointHoverBackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointHoverBorderColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointHoverBorderWidth = table.Column<int>(type: "int", nullable: false),
                    PointHoverRadius = table.Column<int>(type: "int", nullable: false),
                    PointRotation = table.Column<int>(type: "int", nullable: false),
                    PointStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tension = table.Column<int>(type: "int", nullable: false),
                    Fill = table.Column<bool>(type: "bit", nullable: false),
                    BeginAtZeroX = table.Column<bool>(type: "bit", nullable: false),
                    StepSizeX = table.Column<int>(type: "int", nullable: false),
                    MinX = table.Column<int>(type: "int", nullable: false),
                    MaxX = table.Column<int>(type: "int", nullable: false),
                    BeginAtZeroY = table.Column<bool>(type: "bit", nullable: false),
                    StepSizeY = table.Column<int>(type: "int", nullable: false),
                    MinY = table.Column<int>(type: "int", nullable: false),
                    MaxY = table.Column<int>(type: "int", nullable: false),
                    IsXAxisShow = table.Column<bool>(type: "bit", nullable: false),
                    IsYAxisShow = table.Column<bool>(type: "bit", nullable: false),
                    IsLegendShow = table.Column<bool>(type: "bit", nullable: false),
                    GraphId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphStyling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphStyling_Graph_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GraphTableFilter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraphId = table.Column<long>(type: "bigint", nullable: false),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOperator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphTableFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphTableFilter_Graph_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelectedTableColumn",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraphId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedTableColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedTableColumn_Graph_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "GraphType",
                columns: new[] { "Id", "Icon", "IsActive", "Label", "Type" },
                values: new object[,]
                {
                    { 1, "assets/images/icons/bar.svg", true, "Bar Graph", "bar" },
                    { 2, "assets/images/icons/line.svg", true, "Line Graph", "line" },
                    { 3, "assets/images/icons/pie.svg", true, "Pie Graph", "pie" },
                    { 4, "assets/images/icons/doughnut.svg", true, "Doughnut Graph", "doughnut" },
                    { 5, "assets/images/icons/scatter.svg", true, "Scatter Plot", "scatter" },
                    { 6, "assets/images/icons/scatter.svg", true, "Score Card", "score" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardGraph_DashboardId",
                table: "DashboardGraph",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardGraph_GraphId",
                table: "DashboardGraph",
                column: "GraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Graph_TypeId",
                table: "Graph",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphColumn_GraphId",
                table: "GraphColumn",
                column: "GraphId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphStyling_GraphId",
                table: "GraphStyling",
                column: "GraphId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphTableFilter_GraphId",
                table: "GraphTableFilter",
                column: "GraphId");

            migrationBuilder.CreateIndex(
                name: "IX_MergeQueryDetail_MergeQueryId",
                table: "MergeQueryDetail",
                column: "MergeQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedTableColumn_GraphId",
                table: "SelectedTableColumn",
                column: "GraphId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Column");

            migrationBuilder.DropTable(
                name: "DashboardGraph");

            migrationBuilder.DropTable(
                name: "GraphColumn");

            migrationBuilder.DropTable(
                name: "GraphStyling");

            migrationBuilder.DropTable(
                name: "GraphTableFilter");

            migrationBuilder.DropTable(
                name: "KeyValue");

            migrationBuilder.DropTable(
                name: "MergeQueryDetail");

            migrationBuilder.DropTable(
                name: "SelectedTableColumn");

            migrationBuilder.DropTable(
                name: "Dashboard");

            migrationBuilder.DropTable(
                name: "MergeQuery");

            migrationBuilder.DropTable(
                name: "Graph");

            migrationBuilder.DropTable(
                name: "GraphType");
        }
    }
}
