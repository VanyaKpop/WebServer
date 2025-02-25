using Microsoft.AspNetCore.Mvc;
using WebServer.Interfaces;
using WebServer.Models;
using Microsoft.AspNetCore.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Mysqlx;


namespace WebServer.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{

    [HttpGet("Comments/{id}")]
    public async Task<IActionResult> GetComments([FromServices] IDBService service, [FromRoute] long id)
    {
        var comments = await service.GetComments(id);

        if (comments is null) return NotFound();

        return new ObjectResult(comments);
    }

    [HttpPost("PostTest")]
    public IActionResult PostTest([FromServices] IDBService service, [FromBody] TestRequest testRequest)
    {
        if (testRequest is null || testRequest.Author is null || testRequest.Name is null || testRequest.Questions is null) return BadRequest();

        try
        {
            service.AddTest(testRequest);
        }
        catch 
        {
            return BadRequest();
        }

        return Created();
    }

    [HttpGet("GetTests")]
    public async Task<IActionResult> GetAllTests([FromServices] IDBService service)
    {

        var tests = await service.GetTests();
        if (tests is null) return NotFound();

        return new JsonResult(tests);
    }

    [HttpGet("GetTests/{Author}")]
    public async Task<IActionResult> GetTestsByAuthor([FromServices] IDBService service, [FromRoute] string author)
    {

        var tests = await service.GetTests(author);
        if (tests is null) return NotFound();

        return new ObjectResult(tests);
    }
    [HttpGet("GetTest/{id}")]
    public async Task<IActionResult> GetTestsById([FromServices] IDBService service, [FromRoute] long id)
    {

        var tests = await service.GetTest(id);
        if (tests is null) return NotFound();

        return new ObjectResult(tests);
    }
}