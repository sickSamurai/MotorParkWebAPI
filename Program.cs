using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

using MotorPark.Database;
using MotorPark.Services.EnginesDbService;
using MotorPark.Services.UsersDbService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<MotorParkDbContext>(builder.Configuration.GetConnectionString("MotorPark"));
builder.Services.AddScoped<IEngineDbService, EngineDbService>();
builder.Services.AddScoped<IUsersDbService, UsersDbService>();
builder.Services.AddCors(options => {
  options.AddPolicy(name: "DefaultPolicy", policy => {
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("DefaultPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
