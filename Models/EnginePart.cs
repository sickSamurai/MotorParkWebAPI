namespace MotorPark.Models {
  public class EnginePart {    
    public EnginePartType Type { get; set; } = new EnginePartType();
    public int Units { get; set; } = 0;
  }
}
