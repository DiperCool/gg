using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Application.Profiles.Command.UpdatePassword;
using CleanArchitecture.Application.Stages.Command.CreateStage;
using CleanArchitecture.Application.Stages.Command.UpdateStage;
using CleanArchitecture.Application.Stages.Query.GetStages;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class StageController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateStage(CreateStageCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStage(UpdateStageCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet("{eventId}")]
    [ProducesResponseType(typeof(List<StageDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStages(string eventId)
    {
        return Ok(await Mediator.Send(new GetStagesQuery()
        {
            EventId = eventId
        }));
    }
}