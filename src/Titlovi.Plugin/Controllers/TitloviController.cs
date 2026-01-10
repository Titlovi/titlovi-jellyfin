using System.Net.Mime;
using MediaBrowser.Common.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Titlovi.Plugin.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Policy = Policies.SubtitleManagement)]
public sealed class TitloviController : ControllerBase
{
  [HttpPost("GetToken")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult> GetToken()
  {
    return Ok();
  }

  [HttpPost("ValidateLogin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult> ValidateLogin()
  {
    return Ok();
  }
}
