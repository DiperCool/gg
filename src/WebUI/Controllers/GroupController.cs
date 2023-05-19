using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Application.Groups.Command.CreateGroup;
using CleanArchitecture.Application.Groups.Command.JoinGroup;
using CleanArchitecture.Application.Groups.Command.UpdateGroup;
using CleanArchitecture.Application.Groups.Query.GetGroups;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class GroupController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateGroup(CreateGroupCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinGroup(JoinGroupCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGroup(UpdateGroupCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet("{stageId}")]
    [ProducesResponseType(typeof(List<GroupDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroups(string stageId)
    {
        return Ok(await Mediator.Send(new GetGroupsQuery()
        {
            StageId = stageId
        }));
    }
}