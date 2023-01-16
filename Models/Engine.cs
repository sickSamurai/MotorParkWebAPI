namespace MotorPark.Models {
  public class Engine {
    public string? Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public decimal ImportValue { get; set; } = 0;
    public decimal Power { get; set; } = 0;
    public EnginePart[] Parts { get; set; } = Array.Empty<EnginePart>();
  }
}
