namespace UserSystem.Models.Helper;


public class PagedList<T> : List<T>
{
    public PagedList(List<T> items, ulong count, ulong pageNumber, ulong pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (ulong)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public ulong CurrentPage { get; }
    public ulong TotalPages { get; }
    public ulong PageSize { get; }
    public ulong TotalCount { get; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public static Task<PagedList<T>> ToPagedList(List<T> source, ulong pageNumber, ulong pageSize)
    {
        var count = source.Count;
        var items = source.Skip((int)((pageNumber - 1) * pageSize))
            .Take((int)pageSize)
            .ToList();

        return Task.FromResult(new PagedList<T>(items, (ulong)count, pageNumber, pageSize));
    }
}