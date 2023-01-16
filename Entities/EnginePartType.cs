using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorPark.Entities {
  
  [Table("EnginePartType")]
  public class EnginePartType {

    [Key] public string Id { get; set; } = "";
    [Required] public string Name { get; set; } = "";
    public string? Description { get; set; }

    public override bool Equals(object? obj) {
      return obj is EnginePartType dTO && Id == dTO.Id;
    }

    public override int GetHashCode() {
      return HashCode.Combine(Id);
    }
  }
}
