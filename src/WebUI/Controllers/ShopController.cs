using CleanArchitecture.Application.Common.DTOs.ShopItems;
using CleanArchitecture.Application.Shop.Command.BuyShopItem;
using CleanArchitecture.Application.Shop.Command.CreateShopItem;
using CleanArchitecture.Application.Shop.Command.UpdateShopItem;
using CleanArchitecture.Application.Shop.Query.GetExtendedShopItems;
using CleanArchitecture.Application.Shop.Query.GetShopItem;
using CleanArchitecture.Application.Shop.Query.GetShopItems;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class ShopController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> BuyShopItem([FromBody] BuyShopItemCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateShopItem([FromBody] CreateShopItemCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]

    public async Task<IActionResult> UpdateShopItem([FromBody] UpdateShopItemCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<PublicShopItemDTO>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetShopItems([FromQuery] GetShopItemsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<ShopItemDTO>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetExtendedShopItems([FromQuery] GetExtendedShopItemsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
    [HttpGet]
    [ProducesResponseType(typeof(PublicShopItemDTO), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetShopItem([FromQuery] GetShopItemQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}