using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace MotorPark.Entities {
  
  [Table("EnginePart")]
  [PrimaryKey("EngineId", "TypeId")]
  public class EnginePart {

    public string EngineId { get; set; } = "";


    public string TypeId { get; set; } = "";


    [Required] public int Units { get; set; } = 0;

    public override bool Equals(object? obj) {
      return obj is EnginePart dTO && EngineId == dTO.EngineId && TypeId == dTO.TypeId;
    }

    public override int GetHashCode() {
      return HashCode.Combine(EngineId, TypeId);
    }
  }
}
