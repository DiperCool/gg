using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.News.Command.CreateNews;
using CleanArchitecture.Application.News.Command.DeleteNews;
using CleanArchitecture.Application.News.Command.DeleteNewsPictures;
using CleanArchitecture.Application.News.Command.EditNews;
using CleanArchitecture.Application.News.Command.UpdateNewsPicture;
using CleanArchitecture.Application.News.Command.UploadPicture;
using CleanArchitecture.Application.News.Query;
using CleanArchitecture.Application.News.Query.GetNews;
using CleanArchitecture.Application.News.Query.GetNewsByNewsEditorId;
using CleanArchitecture.Application.Organizers.Commands.UpdateLogoOrganizer;
using CleanArchitecture.WebUI.ExtensionsMethods;
using CleanArchitecture.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class NewsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateNews([FromForm] CreateNewsModel model)
    {
        return Ok(await Mediator.Send(new CreateNewsCommand()
        {
            Title = model.Title,
            Background = model.Background==null? null : await model.Background.ConvertToFileModelAsync(),
            Content = model.Content,
            Type = model.Type,
            Pictures = model.Pictures
        }));
    }
    [HttpPost]
    [ProducesResponseType(typeof(MediaFileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadPicture([FromForm]UploadPictureModel model)
    {
        if (model.Picture == null)
        {
            throw new BadRequestException("Picture can't be empty");
        }
        return Ok(await Mediator.Send(new UploadPictureCommand()
        {
            Picture = await model.Picture.ConvertToFileModelAsync(),
            NewsId = model.NewsId
        }));
    }
    [HttpPost]
    [ProducesResponseType(typeof(MediaFileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateBackground([FromForm] UpdateBackgroundModel model )
    {
        return Ok(await Mediator.Send(new UpdateBackgroundCommand()
        {
            NewsId = model.NewsId,
            File = model.File==null? null: await model.File.ConvertToFileModelAsync()
        }));
    }
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditNews([FromBody] EditNewsCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpDelete]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNews([FromBody] DeleteNewsCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePictures([FromBody] DeleteNewsPicturesCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<NewsDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNews([FromQuery] GetNewsQuery command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<NewsDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNewsByNewsEditorId([FromQuery] GetNewsByNewsEditorIdQuery command)
    {
        return Ok(await Mediator.Send(command));
    }
}