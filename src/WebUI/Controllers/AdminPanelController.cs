using CleanArchitecture.Application.AdminPanel.Command.CreateEmployee;
using CleanArchitecture.Application.AdminPanel.Command.DeleteEmployee;
using CleanArchitecture.Application.AdminPanel.Command.DeleteUser;
using CleanArchitecture.Application.AdminPanel.Command.GiveBan;
using CleanArchitecture.Application.AdminPanel.Command.GiveCoins;
using CleanArchitecture.Application.AdminPanel.Command.GiveShadowBan;
using CleanArchitecture.Application.AdminPanel.Command.Login;
using CleanArchitecture.Application.AdminPanel.Command.RefreshTokens;
using CleanArchitecture.Application.AdminPanel.Command.RestoreEmployee;
using CleanArchitecture.Application.AdminPanel.Command.RestoreUser;
using CleanArchitecture.Application.AdminPanel.Command.Unban;
using CleanArchitecture.Application.AdminPanel.Command.UnShadowBan;
using CleanArchitecture.Application.AdminPanel.Query.GetLogs;
using CleanArchitecture.Application.AdminPanel.Query.GetLogsByEmployeeId;
using CleanArchitecture.Application.AdminPanel.Query.GetMe;
using CleanArchitecture.Application.AdminPanel.Query.GetUser;
using CleanArchitecture.Application.AdminPanel.Query.GetUsers;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.News.Command.UploadPicture;
using CleanArchitecture.WebUI.ExtensionsMethods;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;
public class AdminPanelController: ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(EmployeeLoginCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokens(EmployeeRefreshTokensCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GiveCoins(GiveCoinsCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet]
    [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe()
    {
        return Ok(await Mediator.Send(new EmployeeGetMeQuery()));
    }
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<UserDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query )
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromQuery] GetUserQuery query )
    {
        return Ok(await Mediator.Send(query));
    }
    
    [HttpDelete]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(DeleteUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpDelete]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteEmployee(DeleteEmployeeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> RestoreUser(RestoreUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> RestoreEmployee(RestoreEmployeeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GiveBan(GiveBanCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Unban(UnbanCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GiveShadowBan(GiveShadowBanCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UnShadowBan(UnShadowBanCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<EmployeeLogDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeLogs([FromQuery] GetLogsQuery query )
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<EmployeeLogDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLogsByEmployeeId([FromQuery] GetLogsByEmployeeIdQuery query )
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpPost]
    [ProducesResponseType(typeof(MediaFileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadPicture(IFormFile? picture)
    {
        if (picture == null)
        {
            throw new BadRequestException("Image can not be empty");
        }
        return Ok(await Mediator.Send(new CleanArchitecture.Application.AdminPanel.Command.UploadPicture.UploadPictureCommand()
        {
            Picture = await picture.ConvertToFileModelAsync()
        }));
    }
}