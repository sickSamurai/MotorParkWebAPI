using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using MotorPark.Database;
using MotorPark.Entities;
using MotorPark.Models;

namespace MotorPark.Services.EnginesDbService {
  public class EngineDbService : IEngineDbService {
    private MotorParkDbContext DbContext;

    public EngineDbService(MotorParkDbContext DbContext) {
      this.DbContext = DbContext;
    }

    public async Task<Models.Engine[]> GetEnginesByUser(string UserId) {
      var EnginesOnDB = await DbContext.EngineSet.Where(EngineOnDb => EngineOnDb.UserId == UserId).ToListAsync();
      var EnginesList = new List<Models.Engine>();

      foreach(var EngineDTO in EnginesOnDB) {
        EnginesList.Add(new Models.Engine {
          Id = EngineDTO.Id,
          Name = EngineDTO.Name,
          Description = EngineDTO.Description,
          ImportValue = EngineDTO.ImportValue,
          Power = EngineDTO.Power,
          Parts = await GetPartsByEngine(EngineDTO.Id)
        });
      }

      return EnginesList.ToArray();
    }

    public async Task<EngineResponse> AddEngine(Models.Engine Engine, string UserId) {
      Entities.Engine DataTransferObject = new() {
        Id = Guid.NewGuid().ToString(),
        UserId = UserId,
        Name = Engine.Name,
        Description = Engine.Description,
        ImportValue = Engine.ImportValue,
        Power = Engine.Power
      };

      DbContext.EngineSet.Add(DataTransferObject);
      foreach(var Part in Engine.Parts) AddEnginePart(Part, DataTransferObject.Id);
      await DbContext.SaveChangesAsync();

      return new EngineResponse { CreatedEngineId = DataTransferObject.Id };
    }

    public async Task EditEngine(Models.Engine EngineToEdit) {
      if(EngineToEdit.Id == null) throw new Exception("Engine Id must be provided");

      var DTO = await DbContext.EngineSet.AsNoTracking().SingleOrDefaultAsync(EngineOnDb => EngineOnDb.Id == EngineToEdit.Id);
      if(DTO == null) throw new Exception($"Can't find a Engine with ${EngineToEdit.Id} Id");

      await DeleteEngineParts(DTO.Id);
      foreach(var Part in EngineToEdit.Parts) AddEnginePart(Part, DTO.Id);

      DTO = new Entities.Engine {
        Id = DTO.Id,
        UserId = DTO.UserId,
        Name = EngineToEdit.Name,
        Description = EngineToEdit.Description,
        ImportValue = EngineToEdit.ImportValue,
        Power = EngineToEdit.Power
      };

      DbContext.EngineSet.Update(DTO);
      await DbContext.SaveChangesAsync();
    }

    public async Task DeleteEngine(string EngineId) {
      await DeleteEngineParts(EngineId);
      var EngineToDelete = await DbContext.EngineSet.FindAsync(EngineId);
      if (EngineToDelete!= null) DbContext.Remove(EngineToDelete);
      await DbContext.SaveChangesAsync();
    }    

    public async Task<Models.EnginePart[]> GetPartsByEngine(string EngineId) {
      List<Models.EnginePart> PartsList = new();
      var PartsDTOList = await DbContext.PartsSet.Where(part => part.EngineId == EngineId).ToListAsync();

      foreach(var PartDTO in PartsDTOList)
        PartsList.Add(new Models.EnginePart {
          Type = await GetTypeById(PartDTO.TypeId),
          Units = PartDTO.Units
        });

      return PartsList.ToArray();
    }

    public void AddEnginePart(Models.EnginePart EnginePart, string EngineId) {
      if(EnginePart.Type.Id == null) throw new Exception("Type id must be provided");
      DbContext.PartsSet.Add(new Entities.EnginePart {
        EngineId = EngineId,
        TypeId = EnginePart.Type.Id,
        Units = EnginePart.Units
      });
    }

    public async Task DeleteEngineParts(string EngineId) {
      DbContext.PartsSet.Where(part => part.EngineId == EngineId).ToList().ForEach(part => DbContext.PartsSet.Remove(part));
      await DbContext.SaveChangesAsync();
    }

    public async Task<Models.PartType[]> GetAllTypes() {
      List<Models.PartType> Types = new();
      var TypesDTO = await DbContext.TypesSet.ToArrayAsync();

      foreach(var TypeDTO in TypesDTO)
        Types.Add(new Models.PartType { Id = TypeDTO.Id, Name = TypeDTO.Name, Description = TypeDTO.Description });

      return Types.ToArray();
    }

    public async Task<Models.PartType> GetTypeById(string TypeId) {
      var PartTypeDTO = await DbContext.TypesSet.FindAsync(TypeId);
      if(PartTypeDTO == null) throw new Exception("That Type not exists in the db");
      return new Models.PartType { Id = PartTypeDTO.Id, Name = PartTypeDTO.Name, Description = PartTypeDTO.Description };
    }

    public async Task<string> AddType(Models.PartType PartType) {
      PartType.Id = Guid.NewGuid().ToString();

      DbContext.TypesSet.Add(new Entities.PartType {
        Id = PartType.Id,
        Name = PartType.Name,
        Description = PartType.Description
      });

      await DbContext.SaveChangesAsync();

      return PartType.Id;
    }

    public async Task EditType(Models.PartType Type) {
      if(Type.Id == null) throw new Exception("Id must be provided");
      Entities.PartType TypeDTO = new Entities.PartType { Id = Type.Id, Name = Type.Name, Description = Type.Description };
      DbContext.Update(TypeDTO);
      await DbContext.SaveChangesAsync();
    }

    public async Task RemoveType(string TypeId) {
      DbContext.TypesSet.Remove(new Entities.PartType { Id = TypeId });
      await DbContext.SaveChangesAsync();
    }

  }
}
