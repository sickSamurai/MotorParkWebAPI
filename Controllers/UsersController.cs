using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MotorPark.Models;
using MotorPark.Services.UsersDbService;

namespace MotorPark.Controllers
{
  [EnableCors("DefaultPolicy")]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase {

    private IUsersDbService dbService;

    public UsersController(IUsersDbService dbService) {
      this.dbService = dbService;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] string Id) {
      var User = await dbService.getUserById(Id);
      return Ok(User);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(User User) {
      var registerData = await dbService.RegisterUser(User);
      return Ok(registerData);
    }
    
    [HttpPut]
    public async Task<IActionResult> EditUser(User user) {
      var editionData = await dbService.EditUser(user);
      return Ok(editionData);
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromHeader] string Email, [FromHeader] string Password) {
      var loginData = await dbService.ConfirmLogin(Email, Password);
      return Ok(loginData);
    }
  }
}
