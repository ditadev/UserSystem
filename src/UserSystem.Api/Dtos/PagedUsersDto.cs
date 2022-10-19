using UserSystem.Models;

namespace UserSystem.Api.Dtos;

public class PagedUsersDto : PaginatedDto<List<User>>
{
    public PagedUsersDto(List<User> data, PageInformation pageInfo) : base(data, pageInfo)
    {
    }
}