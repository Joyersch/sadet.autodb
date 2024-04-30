using Microsoft.AspNetCore.Mvc;
using sadet.autodb.api.Interfaces;
using sadet.autodb.api.Models;

namespace sadet.autodb.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : Controller
{
    private readonly IGamesRepository _repository;

    public GamesController(IGamesRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet("{appid}")]
    [ProducesResponseType(200, Type = typeof(Game))]
    [ProducesResponseType(400)]
    public IActionResult GetData(int appid)
    {
        if (!_repository.Exists(appid))
            return NotFound();
        return Ok(_repository.Get(appid));
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Game>))]
    public IActionResult GetData()
    {
        return Ok(_repository.Get());
    }
}