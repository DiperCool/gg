using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Organizers.Commands.CreateOrganizer;
using CleanArchitecture.Application.Organizers.Commands.EditProfileOrganizer;
using CleanArchitecture.Application.Organizers.Commands.UpdateLogoOrganizer;
using CleanArchitecture.WebUI.ExtensionsMethods;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;
public class OrganizerController: ApiControllerBase
{
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditProfile(EditProfileOrganizerCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(MediaFileDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLogo(IFormFile? picture)
    {
        return Ok(await Mediator.Send(new UpdateLogoOrganizerCommand()
        {
            File =picture==null?null:await picture.ConvertToFileModelAsync()
        }));
    }
}