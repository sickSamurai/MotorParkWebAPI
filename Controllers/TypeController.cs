using Microsoft.AspNetCore.Mvc;

using MotorPark.Models;
using MotorPark.Services;

namespace MotorPark.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class TypeController : ControllerBase {

    private IEngineDbService DbService;

    public TypeController(IEngineDbService DbService) {
      this.DbService = DbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTypes() {
      return Ok(await DbService.GetAllTypes());
    }


    [HttpPost]
    public async Task<IActionResult> AddTypeAsync([FromBody] EnginePartType Type) {
      return Ok(await DbService.AddType(Type));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveType([FromRoute] string TypeID) {
      await DbService.RemoveType(TypeID);
      return Ok();
    }    

    [HttpPut]
    public async Task<IActionResult> EditType([FromBody] EnginePartType Type) {
      await DbService.EditType(Type);
      return Ok();
    }

  }
}
