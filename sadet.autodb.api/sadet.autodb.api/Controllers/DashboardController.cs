using Microsoft.AspNetCore.Mvc;
using sadet.autodb.api.Interfaces;
using sadet.autodb.api.Models;
using sadet.autodb.api.Repository;

namespace sadet.autodb.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : Controller
{
    private readonly IDashboardRepository _repository;


    public DashboardController(IDashboardRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetData()
    {
        // Use GetData in case GetDataFast returns bad values
        var tableRows = _repository.GetDataFast();
        return Ok(tableRows);
    }
}