using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Application.Events.Command.ApproveEvent;
using CleanArchitecture.Application.Events.Command.CancelParticipation;
using CleanArchitecture.Application.Events.Command.CreateEvent;
using CleanArchitecture.Application.Events.Command.JoinEvent;
using CleanArchitecture.Application.Events.Command.SetApprovedParticipant;
using CleanArchitecture.Application.Events.Command.UpdateEvent;
using CleanArchitecture.Application.Events.Command.UpdateEventPicture;
using CleanArchitecture.Application.Events.Query.GetEvent;
using CleanArchitecture.Application.Events.Query.GetEventFilter;
using CleanArchitecture.Application.Events.Query.GetEvents;
using CleanArchitecture.Application.Events.Query.GetEventsOrganizer;
using CleanArchitecture.WebUI.ExtensionsMethods;
using CleanArchitecture.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class EventController: ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveEvent([FromBody] ApproveEventCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(MediaFileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEventPicture([FromForm] UpdateEventPictureModel model)
    {
        return Ok(await Mediator.Send(new UpdateEventPictureCommand()
        {
            EventId = model.EventId,
            File = model.Picture == null ? null : await model.Picture.ConvertToFileModelAsync()
        }));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinEventCommand([FromBody] JoinEventCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelParticipation([FromBody] CancelParticipationCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> SetApprovedParticipant([FromBody] SetApprovedParticipantCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]

    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<EventWithoutStageDTO>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetEvents()
    {
        return Ok(await Mediator.Send(new GetEventsQuery()));
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EventDTO), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetEvent(string id)
    {
        return Ok(await Mediator.Send(new GetEventQuery()
        {
            Id = id
        }));
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<EventWithoutStageDTO>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetEventsFilter([FromQuery] GetEventFilterQuery query )
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet("{organizerId:guid}")]
    [ProducesResponseType(typeof(List<EventWithoutStageDTO>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetEventsOrganizer(Guid organizerId)
    {
        return Ok(await Mediator.Send(new GetEventsOrganizerQuery
        {
            OrganizerId = organizerId
        }));
    }
    
}