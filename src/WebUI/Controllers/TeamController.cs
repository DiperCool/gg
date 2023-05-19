using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Teams.Command.CreateTeam;
using CleanArchitecture.Application.Teams.Command.JoinManagerTeam;
using CleanArchitecture.Application.Teams.Command.JoinTeam;
using CleanArchitecture.Application.Teams.Command.LeaveTeam;
using CleanArchitecture.Application.Teams.Command.RemoveTeam;
using CleanArchitecture.Application.Teams.Command.UpdateLogoTeam;
using CleanArchitecture.Application.Teams.Command.UpdateTeam;
using CleanArchitecture.Application.Teams.Query.GetInvitationCodeLink;
using CleanArchitecture.Application.Teams.Query.GetManagerInvitationCodeLink;
using CleanArchitecture.Application.Teams.Query.GetMyTeams;
using CleanArchitecture.Application.Teams.Query.GetTeam;
using CleanArchitecture.Application.Teams.Query.GetTeamates;
using CleanArchitecture.Application.Teams.Query.GetTeamByInvitationCode;
using CleanArchitecture.WebUI.ExtensionsMethods;
using CleanArchitecture.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class TeamController: ApiControllerBase
{

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromForm] CreateTeamModel model)
    {
        return Ok(await Mediator.Send(new CreateTeamCommand()
        {
            Title = model.Title,
            Tag=model.Tag,
            Picture = model.Picture == null? null: await model.Picture.ConvertToFileModelAsync()
        }));
    }
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody]UpdateTeamCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLogo([FromForm] UpdateLogoTeamModel model )
    {
        return Ok(await Mediator.Send(new UpdateLogoTeamCommand()
        {
            TeamId = model.TeamId,
            File = model.File==null ? null : await model.File.ConvertToFileModelAsync()
        }));
    }
    [HttpDelete]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTeam([FromBody] RemoveTeamCommand command )
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinTeam([FromBody]JoinTeamCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> LeaveTeam([FromBody]LeaveTeamCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinTeamManager([FromBody]JoinManagerTeamCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<TeamDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyTeams()
    {
        return Ok(await Mediator.Send(new GetMyTeamsQuery()));
    }
    [HttpGet("{teamId:guid}")]
    [ProducesResponseType(typeof(InvitationCodeModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvitationCodeLink(Guid teamId)
    {
        return Ok(await Mediator.Send(new GetInvitationCodeLinkQuery()
        {
            TeamId = teamId
        }));
    }
    [HttpGet("{teamId:guid}")]
    [ProducesResponseType(typeof(List<TeammateDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeammates(Guid teamId)
    {
        return Ok(await Mediator.Send(new GetTeammatesQuery()
        {
            TeamId = teamId
        }));
    }
    [HttpGet("{teamId:guid}")]
    [ProducesResponseType(typeof(InvitationCodeModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetManagerInvitationCodeLinkQuery(Guid teamId)
    {
        return Ok(await Mediator.Send(new GetManagerInvitationCodeLinkQuery()
        {
            TeamId = teamId
        }));
    }
    
    [HttpGet("{teamId:guid}")]
    [ProducesResponseType(typeof(TeamDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeam(Guid teamId)
    {
        return Ok(await Mediator.Send(new GetTeamQuery()
        {
            TeamId = teamId
        }));
    }
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(TeamDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeamByInvitationCode(string code)
    {
        return Ok(await Mediator.Send(new GetTeamByInvitationCodeQuery()
        {
            Code = code
        }));
    }
}