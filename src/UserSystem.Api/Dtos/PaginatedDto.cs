namespace UserSystem.Api.Dtos;

public abstract class PaginatedDto<TResultDto> : Dto<TResultDto> where TResultDto: class
{
    public PageInformation PageInfo { get; set; }

    protected PaginatedDto(AppFaultCode code, string message, TResultDto data, PageInformation pageInfo) : base(code, message, data)
    {
        PageInfo = pageInfo;
    }

    public class PageInformation
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }
}