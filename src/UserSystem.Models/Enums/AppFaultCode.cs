namespace UserSystem.Models.Enums;

public enum AppFaultCode
{
    Success = 0,
    ServerError = 1,
    UserInputError = 2,
    UserNotAuthenticated = 3,
    UserLacksPrivileges = 4
}