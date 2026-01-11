using System.Net;
using System.Net.Mime;
using MediaBrowser.Common.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Titlovi.Api;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Plugin.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Policy = Policies.SubtitleManagement)]
public sealed class TitloviController(IKodiClient kodiClient) : ControllerBase
{
    [HttpPost("GetToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetToken([FromBody] LoginRequest? request)
    {
        if (request is null)
            return BadRequest();

        try
        {
            var token = await kodiClient.GetToken(request.Username, request.Password).ConfigureAwait(false);
            return Ok(token);
        }
        catch (Refit.ApiException ex)
        {
            var content = await ex.GetContentAsAsync<string>().ConfigureAwait(false);
            return StatusCode((int)ex.StatusCode, new { error = content });
        }
    }

    [HttpPost("ValidateLogin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ValidateLogin(string username, string password)
    {
        try
        {
            var response = await kodiClient.ValidateLogin(new()
            {
                Username = username,
                Password = password
            }).ConfigureAwait(false);

            return Ok(response.StatusCode == HttpStatusCode.OK);
        }
        catch (Refit.ApiException ex)
        {
            var content = await ex.GetContentAsAsync<string>().ConfigureAwait(false);
            return StatusCode((int)ex.StatusCode, new { error = content });
        }
    }
}