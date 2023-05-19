using CleanArchitecture.Application.Authenticate.Command.GenerateRestorePassword;
using CleanArchitecture.Application.Authenticate.Command.Login;
using CleanArchitecture.Application.Authenticate.Command.RefreshTokens;
using CleanArchitecture.Application.Authenticate.Command.Register;
using CleanArchitecture.Application.Authenticate.Command.RestorePassword;
using CleanArchitecture.Application.Authenticate.Query.GetMe;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CleanArchitecture.WebUI.Controllers;
public class AuthenticateController: ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [OpenApiOperation("Sends an email to restore user's password", "Sends an email to restore user's password")]
    public async Task<IActionResult> GenerateRestorePassword(GenerateRestorePasswordCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    public async Task<IActionResult> RestorePassword(RestorePasswordCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]

    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe()
    {
        return Ok(await Mediator.Send(new AuthUserQuery ()));
    }
}