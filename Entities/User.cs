using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorPark.Entities {
  [Table("User")]
  public class User {
    [Key] public string Id { get; set; } = "";
    [Required] public string Username { get; set; } = "";
    [Required] public string Email { get; set; } = "";
    [Required] public string Password { get; set; } = "";

    public override bool Equals(object? obj) {
      return obj is User user && Id == user.Id;
    }

    public override int GetHashCode() {
      return HashCode.Combine(Id);
    }
  }
}
