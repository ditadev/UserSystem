using UserSystem.Models;

namespace UserSystem.Api.Dtos;

public class UserListDto : PaginatedDto<List<User>>
{
    public UserListDto(AppFaultCode code, string message, List<User> data, PageInformation pageInfo) : base(code, message, data, pageInfo)
    {
    }
}