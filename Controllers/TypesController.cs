using Microsoft.AspNetCore.Mvc;

using MotorPark.Models;
using MotorPark.Services.EnginesDbService;

namespace MotorPark.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TypesController : ControllerBase {

    private IEngineDbService DbService;

    public TypesController(IEngineDbService DbService) {
      this.DbService = DbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTypes() {
      return Ok(await DbService.GetAllTypes());
    }


    [HttpPost]
    public async Task<IActionResult> AddTypeAsync([FromBody] PartType Type) {
      return Ok(new { createdTypeId = await DbService.AddType(Type) });
    }

    [HttpPut]
    public async Task<IActionResult> EditType([FromBody] PartType Type) {
      await DbService.EditType(Type);
      return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveType([FromRoute] string TypeId) {
      await DbService.RemoveType(TypeId);
      return Ok();
    }    
  }
}
