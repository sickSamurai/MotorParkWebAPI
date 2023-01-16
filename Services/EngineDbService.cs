using Microsoft.EntityFrameworkCore;

using MotorPark.Database;
using MotorPark.Entities;
using MotorPark.Models;

namespace MotorPark.Services {
  public class EngineDbService : IEngineDbService {    
    private MotorParkDbContext DbContext;

    public EngineDbService(MotorParkDbContext DbContext) {
      this.DbContext = DbContext;
    }

    public async Task<string> AddEngine(Models.Engine Engine) {
      List<Task> Tasks = new();

      Entities.Engine DataTransferObject = new Entities.Engine {
        Id = Guid.NewGuid().ToString(),
        Name = Engine.Name,
        Description = Engine.Description,
        ImportValue = Engine.ImportValue,
        Power = Engine.Power
      };

      DbContext.EngineSet.Add(DataTransferObject);
      await DbContext.SaveChangesAsync();
      foreach(var Part in Engine.Parts) Tasks.Add(AddEnginePart(Part, DataTransferObject.Id));
      await Task.WhenAll(Tasks);
      
      return DataTransferObject.Id;
    }

    public async Task EditEngine(Models.Engine EngineToEdit) {
      if(EngineToEdit.Id == null) throw new Exception("Engine Id must be provided");
      List<Task> UpdateTasks = new();
      
      Entities.Engine DataTransferObject = new Entities.Engine {
        Id = EngineToEdit.Id,
        Name = EngineToEdit.Name,
        Description = EngineToEdit.Description,
        ImportValue = EngineToEdit.ImportValue,
        Power = EngineToEdit.Power
      };

      DbContext.Update(DataTransferObject);
      await DbContext.SaveChangesAsync();

      foreach(var Part in EngineToEdit.Parts) UpdateTasks.Add(EditEnginePart(Part, EngineToEdit.Id));
            
      await Task.WhenAll(UpdateTasks);
    }


    public async Task DeleteEngine(string EngineId) {
      DbContext.PartsSet.Where(part => part.EngineId == EngineId).ToList().ForEach(part => DbContext.PartsSet.Remove(part));
      await DbContext.SaveChangesAsync();
      DbContext.EngineSet.Where(engine => engine.Id == EngineId).ToList().ForEach(engine => DbContext.EngineSet.Remove(engine));
      await DbContext.SaveChangesAsync();

    }

    public async Task<Models.Engine[]> GetAllEngines() {
      var EnginesOnDB = await DbContext.EngineSet.ToListAsync();
      var EnginesList = new List<Models.Engine>();

      foreach(var EngineDTO in EnginesOnDB) {
        EnginesList.Add(new Models.Engine {
          Id = EngineDTO.Id,
          Name = EngineDTO.Name,
          Description = EngineDTO.Description,
          ImportValue = EngineDTO.ImportValue,
          Power = EngineDTO.Power,
          Parts = await GetPartsByMotor(EngineDTO.Id)
        });
      }

      return EnginesList.ToArray();
    }

    public async Task AddEnginePart(Models.EnginePart EnginePart, string EngineId) {
      if(EnginePart.Type.Id == null) throw new Exception("Type id must be provided");
      Entities.EnginePart DataTransferObject = new Entities.EnginePart {
        EngineId = EngineId,
        TypeId = EnginePart.Type.Id,
        Units = EnginePart.Units
      };
      
      DbContext.PartsSet.Add(DataTransferObject);

      await DbContext.SaveChangesAsync();     
    }

    public async Task<Models.EnginePart[]> GetPartsByMotor(string EngineId) {
      List<Models.EnginePart> PartsList = new();
      var PartsDTOList = await DbContext.PartsSet.Where(part => part.EngineId == EngineId).ToListAsync();

      foreach(var PartDTO in PartsDTOList) PartsList.Add(new Models.EnginePart {
        Type = await GetType(PartDTO.TypeId),
        Units = PartDTO.Units
      });

      return PartsList.ToArray();
    }

    public async Task EditEnginePart(Models.EnginePart EnginePart, string EngineId) {
      if(EnginePart.Type.Id == null) throw new Exception("Type id must be provided");
      DbContext.Update(new Entities.EnginePart { EngineId = EngineId, TypeId=EnginePart.Type.Id, Units = EnginePart.Units });
      await DbContext.SaveChangesAsync();
    }

    public async Task DeleteEnginePart(Models.EnginePart EnginePart, string EngineId) {
      if(EnginePart.Type.Id == null) throw new Exception("Type id must be provided");
      DbContext.Remove(new Entities.EnginePart {
        EngineId = EngineId,
        TypeId = EnginePart.Type.Id,
        Units = EnginePart.Units
      });
      
      await DbContext.SaveChangesAsync();
    }       

    public async Task<string> AddType(Models.EnginePartType PartType) {
      PartType.Id = Guid.NewGuid().ToString();

      DbContext.TypesSet.Add(new Entities.EnginePartType {
        Id = PartType.Id,
        Name = PartType.Name,
        Description = PartType.Description
      });

      await DbContext.SaveChangesAsync();

      return PartType.Id;
    }

    public async Task<Models.EnginePartType> GetType(string TypeId) {
      var PartTypeDTO = await DbContext.TypesSet.FindAsync(TypeId);
      if(PartTypeDTO == null) throw new Exception("That Type not exists in the db");
      return new Models.EnginePartType { Id = PartTypeDTO.Id, Name = PartTypeDTO.Name, Description = PartTypeDTO.Description };
    }

    public async Task RemoveType(string TypeId) {
      DbContext.TypesSet.Remove(new Entities.EnginePartType { Id = TypeId });
      await DbContext.SaveChangesAsync();
    }

    public async Task<Models.EnginePartType[]> GetAllTypes() {
      List<Models.EnginePartType> Types = new();
      var TypesDTO = await DbContext.TypesSet.ToArrayAsync();
      
      foreach(var TypeDTO in TypesDTO) 
        Types.Add(new Models.EnginePartType { Id = TypeDTO.Id, Name = TypeDTO.Name, Description = TypeDTO.Description });
      
      return Types.ToArray();
    }

    public async Task EditType(Models.EnginePartType Type) {
      if(Type.Id == null) throw new Exception("Id must be provided");
      Entities.EnginePartType TypeDTO = new Entities.EnginePartType { Id = Type.Id, Name= Type.Name, Description = Type.Description };
      DbContext.Update(TypeDTO);
      await DbContext.SaveChangesAsync();
    }
  }
}
