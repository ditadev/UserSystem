namespace UserSystem.Models.Helper;

public class PageParameters
{
     private const int MaxPageSize = 50;
     private ulong _pageSize = 10;
     public ulong PageNumber { get; set; } = 1;

     public ulong PageSize
     {
          get => _pageSize;
          set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
     }

}