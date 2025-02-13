using Microsoft.AspNetCore.Mvc;
using WebServer.Interfaces;
using WebServer.Models;
using WebServer.Services;

namespace WebServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{

    [HttpPost("PostUser")]
    public async Task<ActionResult> PostUser([FromServices] IDBService service, [FromBody] UserRequest user)
    {
        if (await service.IsUserExist(user.name))
            return BadRequest("User with the same name alredy exist");

        if (user.name == null || user.password == null)
            return BadRequest();

        await Task.Run(() => service.AddUser(user.name, user.password));

        return Created();
    }

    [HttpGet("GetUser/{name}/{password}")]
    public async Task<ActionResult> GetUser([FromServices] IDBService service, string name, string password)
    {
        if (await service.IsUserExist(name, password) == false)
            return NotFound();

        if (await service.IsUserExist(name, password) == true)
            return BadRequest("User with the same name alredy exist");

        if (name == null | password == null)
            return BadRequest();

        return Ok("Authorized");
    }

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
        if (testRequest.key != "1") return BadRequest();

        if (testRequest is null || testRequest.Author is null || testRequest.Name is null || testRequest.DataJson is null) return BadRequest();

        service.AddTest(testRequest);

        return Created();
    }

    [HttpGet("GetTests")]
    public async Task<IActionResult> GetAllTests([FromServices] IDBService service)
    {

        var tests = await service.GetTests();
        if (tests is null) return NotFound();

        return new ObjectResult(tests);
    }

    [HttpGet("GetTests/{Author}")]
    public async Task<IActionResult> GetTestsByAuthor([FromServices] IDBService service, [FromRoute] string author)
    {

        var tests = await service.GetTests(author);
        if (tests is null) return NotFound();

        return new ObjectResult(tests);
    }
}