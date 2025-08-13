using System.Globalization;

namespace ConnektaViz.API;

public class FolderStructureTree
{
    public Guid Key { get; set; }
    public string Label { get; set; }
    public string Icon { get; set; } = string.Empty;
    public bool IsFile { get; set; }
    public string Data { get; set; }
    public IList<FolderStructureTree> Children { get; set; } = new List<FolderStructureTree>();
}

public static class ExtensionHelper
{

    public static bool HasAny(this IEnumerable<object> collection) => collection is not null && collection.Any();

    public static FolderStructureTree BuildTree(this IEnumerable<string> paths)
    {
        var root = new FolderStructureTree { Key = Guid.NewGuid(), Label = "Root" };
        foreach (var path in paths.Order())
        {
            var currentNode = root;
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var existingChild = currentNode.Children.FirstOrDefault(c => c.Label.ToLower() == part.ToLower());
                if (existingChild is null)
                {
                    var newNode = new FolderStructureTree
                    {
                        Key = Guid.NewGuid(),
                        Label = part.ToTitleCase(),
                        Icon = part.Contains('.') ? "pi pi-file" : "pi pi-folder",
                        IsFile = part.Contains('.'),
                        Data = string.IsNullOrEmpty(currentNode.Data) ? part : $"{currentNode.Data}/{part}"
                    };
                    currentNode.Children.Add(newNode);
                    currentNode = newNode;
                }
                else
                {
                    currentNode = existingChild;
                }
            }
        }
        return root;
    }

    public static async Task<IQueryable<T>> ApplyPaginationAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, ResponseDto response)
    {
        response.Total = await query.LongCountAsync();
        response.Success = true;
        return query.Skip((pageNumber > 0 ? pageNumber - 1 : 0) * pageSize).Take(pageSize > 0 ? pageSize : (int)response.Total);
    }

    public static string ToTitleCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }
}
