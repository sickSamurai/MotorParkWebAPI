using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MotorPark.Database;
using MotorPark.Models;
using MotorPark.Services;

namespace MotorPark.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class EngineController : ControllerBase {

    IEngineDbService DbService;

    public EngineController(IEngineDbService DbService) {
      this.DbService = DbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
      return Ok(await DbService.GetAllEngines());
    }

    [HttpPost]
    public async Task<IActionResult> CreateEngine([FromBody] Engine Engine) {
      string CreatedEngineId = await DbService.AddEngine(Engine);
      return Ok(CreatedEngineId);
    }

    [HttpPut]
    public async Task<IActionResult> EditEngine([FromBody] Engine Engine) {
      await DbService.EditEngine(Engine);
      return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEngine(String Id) {
      await DbService.DeleteEngine(Id);
      return Ok();
    }
    
  }
}
