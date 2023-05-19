using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.UserProfiles;
using CleanArchitecture.Application.Profiles.Command.ConfirmEmail;
using CleanArchitecture.Application.Profiles.Command.EditProfile;
using CleanArchitecture.Application.Profiles.Command.ResendEmailConfirmation;
using CleanArchitecture.Application.Profiles.Command.UpdatePassword;
using CleanArchitecture.Application.Profiles.Command.UpdateProfilePicture;
using CleanArchitecture.Application.Profiles.Query.GetProfile;
using CleanArchitecture.WebUI.ExtensionsMethods;
using CleanArchitecture.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CleanArchitecture.WebUI.Controllers;
public class ProfileController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(MediaFileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfilePicture([FromForm]UpdateProfilePictureModel model)
    {
        return Ok(await Mediator.Send(new UpdateProfilePictureCommand()
        {
            File =model.Picture==null?null:await model.Picture.ConvertToFileModelAsync()
        }));
    }
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditProfile(EditProfileCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResendEmailConfirmation()
    {
        return Ok(await Mediator.Send(new ResendEmailConfirmationCommand()));
    }
    [HttpGet]
    [OpenApiOperation("Gets a profile of an authorized user", "Gets a profile of an authorized user")]
    [ProducesResponseType(typeof(PrivateProfileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile()
    {
        return Ok(await Mediator.Send(new GetProfileQuery()));
    }
}