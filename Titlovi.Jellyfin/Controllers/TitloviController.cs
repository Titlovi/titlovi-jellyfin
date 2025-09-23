namespace Titlovi.Jellyfin.Controllers;

using System.Net.Mime;
using MediaBrowser.Common.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Interfaces;

/// <summary>
/// Controller responsible for managing subtitle operations.
/// </summary>
/// <remarks>
/// This controller exposes endpoints for managing subtitles.
/// It requires authorization based on the <c>SubtitleManagement</c> policy.
/// </remarks>
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Policy = Policies.SubtitleManagement)]
public class TitloviController : ControllerBase
{
    private readonly ILogger<TitloviController> logger;
    private readonly ITitloviManager titloviManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TitloviController"/> class.
    /// </summary>
    /// <param name="logger">The logging service used for diagnostics and tracing.</param>
    /// <param name="titloviManager">The service responsible for handling subtitle-related business logic.</param>
    public TitloviController(ILogger<TitloviController> logger, ITitloviManager titloviManager)
    {
        this.logger = logger;
        this.titloviManager = titloviManager;
    }

    /// <summary>
    /// Invalidates the current Titlovi.com authentication token.
    /// </summary>
    /// <remarks>
    /// This endpoint clears the stored token information in the Titlovi configuration
    /// and persists the change.
    /// If the <see cref="TitloviJellyfin"/> instance is not initialized, the method returns
    /// a <c>500 Internal Server Error</c>.
    /// </remarks>
    /// <returns>
    /// Returns:
    /// <list type="bullet">
    ///   <item><description><c>200 OK</c> if the token was successfully invalidated.</description></item>
    ///   <item><description><c>500 Internal Server Error</c> if the <c>TitloviJellyfin</c> instance is not available.</description></item>
    /// </list>
    /// </returns>
    [HttpPost("InvalidateToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult InvalidateToken()
    {
        var titlovi = TitloviJellyfin.Instance;
        if (titlovi is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        titlovi.Configuration.TokenInfo = null;
        titlovi.SaveConfiguration();
        return Ok();
    }
}
