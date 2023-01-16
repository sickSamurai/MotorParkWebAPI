using MotorPark.Models;

namespace MotorPark.Services {
  public interface IEngineDbService {
    Task<string> AddEngine(Engine Engine);
    Task<Engine[]> GetAllEngines();

    Task EditEngine(Engine EngineToEdit);

    Task DeleteEngine(string EngineId);

    Task<string> AddType(EnginePartType EnginePartType);

    Task<EnginePartType> GetType(string TypeId);

    Task<EnginePartType[]> GetAllTypes();

    Task EditType(EnginePartType Type);

    Task RemoveType(string TypeId);
    
  }
}
