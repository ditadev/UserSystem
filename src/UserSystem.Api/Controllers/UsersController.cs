using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using UserSystem.Api.Attributes;
using UserSystem.Api.Dtos;
using UserSystem.Features;
using UserSystem.Models;
using UserSystem.Models.Enums;
using UserSystem.Models.Helper;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[ProducesResponseType(typeof(UserInputErrorResponseDto), 400)]
[ProducesResponseType(typeof(ServerErrorResponseDto), 500)]
[ProducesResponseType(typeof(UserNotAuthenticatedErrorResponseDto), 401)]
[ProducesResponseType(typeof(UserNotPrivilegedResponseDto), 403)]
public class UsersController : AbstractController
{
    private readonly IValidator<LoginUserRequest> _loginUserRequestValidator;
    private readonly IValidator<RegisterUserRequest> _registerUserRequestValidator;
    private readonly IValidator<ResetPasswordRequest> _resetPasswordRequestValidator;
    private readonly IUserService _userService;


    public UsersController(
        IUserService userService,
        IValidator<RegisterUserRequest> registerUserRequestValidator,
        IValidator<LoginUserRequest> loginUserRequestValidator,
        IValidator<ResetPasswordRequest> resetPasswordRequestValidator
    )
    {
        _userService = userService;
        _registerUserRequestValidator = registerUserRequestValidator;
        _loginUserRequestValidator = loginUserRequestValidator;
        _resetPasswordRequestValidator = resetPasswordRequestValidator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmptySuccessResponseDto), 201)]
    public async Task<ActionResult<EmptySuccessResponseDto>> Register(RegisterUserRequest request)
    {
        var result = await _registerUserRequestValidator.ValidateAsync(request);

        if (!result.IsValid) return BadRequest(new UserInputErrorResponseDto(result));

        var user = await _userService.GetUserByEmailAddress(request.EmailAddress);
        var phoneNumber = await _userService.GetUserByPhoneNumber(request.PhoneNumber);

        if (user != null) return BadRequest(new UserInputErrorResponseDto("User Already Exists :("));
        if (phoneNumber != null) return BadRequest(new UserInputErrorResponseDto("Phone Number already registered :("));

        var passwordHash = await _userService.CreatePasswordHash(request.Password);

        await _userService.CreateUser(new User
        {
            EmailAddress = request.EmailAddress,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = passwordHash
        });

        return new EmptySuccessResponseDto("User Created Successfully");
    }

    [HttpPost]
    [ProducesResponseType(typeof(JwtDto), 200)]
    public async Task<ActionResult<JwtDto>> Login(LoginUserRequest request)
    {
        var result = await _loginUserRequestValidator.ValidateAsync(request);

        if (!result.IsValid) return BadRequest(new UserInputErrorResponseDto(result));
        var user = await _userService.GetUserByEmailAddress(request.EmailAddress);

        if (user == null || !await _userService.VerifyPassword(user, request.Password))
            return BadRequest(new UserInputErrorResponseDto("Incorrect Username/Password :("));

        if (user?.VerifiedAt == null) return BadRequest(new UserInputErrorResponseDto("Not Verified :("));

        return Ok(new JwtDto
            (new JwtDto.Credentials { AccessToken = await _userService.CreateJwt(user) }));
    }

    [HttpPatch]
    [ProducesResponseType(typeof(EmptySuccessResponseDto), 200)]
    public async Task<ActionResult<EmptySuccessResponseDto>> VerifyUser(string emailAddress, string token)
    {
        var user = await _userService.GetUserByEmailAddress(emailAddress);
        if (user == null)
            return BadRequest(new UserInputErrorResponseDto("Use a registered Email Address :("));

        if (await _userService.VerifyUser(emailAddress, token) == false)
            return BadRequest(new UserInputErrorResponseDto("Invalid OTP :("));

        return Ok(new EmptySuccessResponseDto("User Verified :)"));
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmptySuccessResponseDto), 200)]
    public async Task<ActionResult<EmptySuccessResponseDto>> ForgotPassword(string emailAddress)
    {
        if (await _userService.ForgotPassword(emailAddress) == false)
            return BadRequest(new UserInputErrorResponseDto("User Not Found :("));

        return Ok(new EmptySuccessResponseDto("You may now Reset your Password :)"));
    }

    [HttpPatch]
    [ProducesResponseType(typeof(EmptySuccessResponseDto), 200)]
    public async Task<ActionResult<EmptySuccessResponseDto>> ResetPassword([FromForm] ResetPasswordRequest request)
    {
        var result = await _resetPasswordRequestValidator.ValidateAsync(request);

        if (!result.IsValid) return BadRequest(new UserInputErrorResponseDto(result));

        if (await _userService.ResetPassword(request.emailAddress, request.Token, request.Password) == false)
            return BadRequest(new UserInputErrorResponseDto("Invalid OTP :("));

        return Ok(new EmptySuccessResponseDto("Password Reset Successful:)"));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedUsersDto), 200)]
    [Authorize(UserRole.Administrator)]
    public async Task<ActionResult<PagedUsersDto>> GetAllUsers([FromQuery] PageParameters pageParameters)
    {
        var users = await _userService.GetAllUsers(pageParameters);
        var userList = new PagedUsersDto(users, new PaginatedDto<List<User>>.PageInformation
        {
            CurrentPage = users.CurrentPage,
            TotalPages = users.TotalPages,
            HasNext = users.HasNext,
            HasPrevious = users.HasPrevious,
            TotalCount = users.TotalCount
        });

        return Ok(userList);
    }}