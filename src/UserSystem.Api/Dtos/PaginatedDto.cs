namespace UserSystem.Api.Dtos;

public abstract class PaginatedDto<TResultDto> : SuccessResponseDto<TResultDto>
{
    protected PaginatedDto(TResultDto data, PageInformation pageInfo) : base(data)
    {
        PageInfo = pageInfo;
    }

    public PageInformation PageInfo { get; set; }

    public class PageInformation
    {
        public ulong TotalCount { get; set; }
        public ulong TotalPages { get; set; }
        public ulong CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }
}