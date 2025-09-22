using System.Net.Mime;
using MediaBrowser.Common.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Titlovi.Jellyfin.Interfaces;

namespace Titlovi.Jellyfin.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Policy = Policies.SubtitleManagement)]
public class TitloviController : ControllerBase
{
    private readonly ILogger<TitloviController> logger;
    private readonly ITitloviManager titloviManager;

    public TitloviController(ILogger<TitloviController> logger, ITitloviManager titloviManager)
    {
        this.logger = logger;
        this.titloviManager = titloviManager;
    }
}
