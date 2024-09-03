using Microsoft.AspNetCore.Mvc;

namespace Reapit.Services.Demo.Api.Controllers.Abstract;

/// <summary>
/// Base controller class for Reapit API controllers.
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public abstract class ReapitApiController : ControllerBase
{
}