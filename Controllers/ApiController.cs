using Microsoft.AspNetCore.Mvc;
using WebServer.Interfaces;
using WebServer.Models;
using WebServer.Services;

namespace WebServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController: ControllerBase
{

    [HttpPost]
    public async Task<ActionResult> PostUser([FromServices] IDBService service, [FromBody] UserRequest user)
    {
        if (await service.IsUserExist(user.name))
            return BadRequest("User with the same name alredy exist");

        if (user.name == null || user.password == null)
            return BadRequest();

        await Task.Run(() => service.AddUser(user.name, user.password));

        return Ok("User add to the database");
    }

    [HttpGet("{name}/{password}")]
    public async Task<ActionResult> Get([FromServices] IDBService service, string name, string password)
    {
         if (await Task.Run(() => service.IsUserExist(name, password)) == false) 
            return NotFound();

        if (await Task.Run(() => service.IsUserExist(name, password)) == true) 
            return BadRequest("User with the same name alredy exist");

        if (name == null || password == null)
            return BadRequest();

        return Ok("Authorized");
    }

}