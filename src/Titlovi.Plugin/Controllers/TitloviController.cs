using System.Net;
using System.Net.Mime;
using MediaBrowser.Common.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Titlovi.Api;
using Titlovi.Api.Models.Requests;

namespace Titlovi.Plugin.Controllers;

/// <summary>
/// API controller for managing Titlovi subtitle service authentication and tokens.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Policy = Policies.SubtitleManagement)]
public sealed class TitloviController(IKodiClient kodiClient) : ControllerBase
{
    /// <summary>
    /// Retrieves an authentication token using username and password credentials.
    /// </summary>
    /// <param name="request">Login credentials containing username and password.</param>
    /// <returns>Authentication token on success, error details on failure.</returns>
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

    /// <summary>
    /// Clears the stored authentication token from plugin configuration.
    /// </summary>
    /// <returns>Success status or internal server error if plugin instance unavailable.</returns>
    [HttpPost("InvalidateToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> InvalidateToken()
    {
        var instance = TitloviPlugin.Instance;
        if (instance is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        instance.Configuration.Token = null;
        instance.SaveConfiguration();
        return Ok();
    }

    /// <summary>
    /// Validates user credentials against the Titlovi service.
    /// </summary>
    /// <param name="username">Username to validate.</param>
    /// <param name="password">Password to validate.</param>
    /// <returns>Boolean indicating whether credentials are valid.</returns>
    [HttpPost("ValidateLogin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ValidateLogin(string username, string password)
    {
        try
        {
            return Ok((await kodiClient.ValidateLogin(new()
            {
                Username = username,
                Password = password
            }).ConfigureAwait(false)).StatusCode == HttpStatusCode.OK);
        }
        catch (Refit.ApiException ex)
        {
            var content = await ex.GetContentAsAsync<string>().ConfigureAwait(false);
            return StatusCode((int)ex.StatusCode, new { error = content });
        }
    }
}