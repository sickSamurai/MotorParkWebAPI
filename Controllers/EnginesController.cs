using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MotorPark.Database;
using MotorPark.Models;
using MotorPark.Services.EnginesDbService;

namespace MotorPark.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EnginesController : ControllerBase {

    IEngineDbService DbService;

    public EnginesController(IEngineDbService DbService) {
      this.DbService = DbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromHeader] string UserId) {
      return Ok(await DbService.GetEnginesByUser(UserId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEngine([FromBody] Engine Engine, [FromHeader] string UserId) {
      var CreatedEngineData = await DbService.AddEngine(Engine, UserId);
      return Ok(CreatedEngineData);
    }

    [HttpPut]
    public async Task<IActionResult> EditEngine([FromBody] Engine Engine) {
      await DbService.EditEngine(Engine);
      return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteEngine([FromRoute] String Id) {
      await DbService.DeleteEngine(Id);
      return Ok();
    }
    
  }
}
