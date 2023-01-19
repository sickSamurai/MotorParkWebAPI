using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorPark.Entities {
  
  [Table("PartType")]
  public class PartType {

    [Key] public string Id { get; set; } = "";
    [Required] public string Name { get; set; } = "";
    public string? Description { get; set; }

    public override bool Equals(object? obj) {
      return obj is PartType dTO && Id == dTO.Id;
    }

    public override int GetHashCode() {
      return HashCode.Combine(Id);
    }
  }
}
