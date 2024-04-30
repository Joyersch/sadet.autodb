using System.Collections;
using Microsoft.AspNetCore.Mvc;
using sadet.autodb.api.Context;
using sadet.autodb.api.Interfaces;
using sadet.autodb.api.Models;

namespace sadet.autodb.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : Controller
{
    private readonly IDataRepository _repository;

    public enum Limit
    {
        Week,
        Month,
        Year,
        AllTime
    }

    public DataController(IDataRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{appid}")]
    [ProducesResponseType(200, Type = typeof(ICollection<Data>))]
    [ProducesResponseType(400)]
    public IActionResult GetData(int appid)
    {
        if (!_repository.Exists(appid))
            return NotFound();
        return Ok(_repository.GetData(appid));
    }

    [HttpGet("{appid}/{limit}")]
    [ProducesResponseType(200, Type = typeof(ICollection<Data>))]
    [ProducesResponseType(400)]
    public IActionResult GetData(int appid, Limit limit)
    {
        if (!_repository.Exists(appid))
            return NotFound();
        return Ok(_repository.GetData(appid, limit switch
        {
            Limit.Week => new TimeSpan(7, 0, 0, 0),
            Limit.Month => new TimeSpan(28, 0, 0, 0, 0),
            Limit.Year => new TimeSpan(365, 0, 0, 0),
            Limit.AllTime => TimeSpan.FromTicks(DateTime.Now.Ticks),
        }));
    }
}