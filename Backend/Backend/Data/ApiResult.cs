namespace Backend.Data;

public class ApiResult<T>
{
    public int Count { get; set; }
    public string? Next { get; set; } = null;
    public string? Previous { get; set; } = null;
    public List<T> Results { get; set; }

    private ApiResult(
        int count,
        string? next,
        string? previous,
        List<T> results)
    {
        Count = count;
        Next = next;
        Previous = previous;
        Results = results;
    }

    public static async Task<ApiResult<T>> CreateAsync(
        List<T> source,
        int offset,
        int limit,
        string url)
    {
        var totalCount = source.Count;
        source = source
            .Skip(offset)
            .Take(limit)
            .ToList();

        var previous = (string?)null;
        var next = (string?)null;

        if (totalCount != 0 && limit > 0 && offset >= 0)
        {
            if (offset + limit < totalCount)
            {
                next = url + $"?limit={limit}&offset={offset + limit}";
            }

            if (offset > 0)
            {
                if (offset - limit > 0)
                {
                    previous = url + $"?limit={limit}&offset={offset - limit}";
                }
                else
                {
                    previous = url + $"?limit={limit}&offset={0}";
                }
            }
        }

        return new ApiResult<T>(
            totalCount,
            next,
            previous,
            source);
    }
}