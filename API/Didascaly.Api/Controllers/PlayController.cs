using Microsoft.AspNetCore.Mvc;
using Didascaly.Core.Interfaces;
using Didascaly.Core.Models;

namespace Didascaly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayController : ControllerBase
{
    private readonly IPlayRepository _playRepository;

    public PlayController(IPlayRepository playRepository)
    {
        _playRepository = playRepository;
    }

    [HttpGet("moliere")]
    public IActionResult GetMoliere()
    {
        Play? play = _playRepository.GetPlay("Le Malade Imaginaire");
        
        if (play == null)
        {
            return NotFound("Pièce introuvable.");
        }
        
        return Ok(play);
    }
}