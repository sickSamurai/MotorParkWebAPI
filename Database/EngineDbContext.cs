using Microsoft.EntityFrameworkCore;

using MotorPark.Entities;
using MotorPark.Models;

namespace MotorPark.Database {
  public class MotorParkDbContext: DbContext{
    public MotorParkDbContext(DbContextOptions<MotorParkDbContext> options): base(options) { }
    public DbSet<Entities.Engine> EngineSet { get; set; }
    
    public DbSet<Entities.EnginePart> PartsSet { get; set; }

    public DbSet<Entities.EnginePartType> TypesSet { get; set; }
    
  }
}
