using MotorPark.Models;

namespace MotorPark.Services.EnginesDbService {
  public interface IEngineDbService {

    Task<Engine[]> GetEnginesByUser(string UserId);

    Task<EngineResponse> AddEngine(Engine Engine, string UserId);

    Task EditEngine(Engine EngineToEdit);

    Task DeleteEngine(string EngineId);

    Task<PartType[]> GetAllTypes();

    Task<PartType> GetTypeById(string TypeId);

    Task<string> AddType(PartType PartType);    

    Task EditType(PartType Type);

    Task RemoveType(string TypeId);

  }
}
