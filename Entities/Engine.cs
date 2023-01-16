using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace MotorPark.Entities {
  
  [Table("Engine")]
  public class Engine {

    [Key] public string Id { get; set; } = "";


    [Required] public string Name { get; set; } = "";
    
    public string? Description { get; set; }

    [Required] public decimal ImportValue { get; set; } = 0;

    [Required] public decimal Power { get; set; } = 0;          

    public override bool Equals(object? obj) {
      return obj is Engine dTO && Id == dTO.Id;
    }

    public override int GetHashCode() {
      return HashCode.Combine(Id);
    }
  }
}
