using Microsoft.EntityFrameworkCore;

using MotorPark.Database;
using MotorPark.Models;

namespace MotorPark.Services.UsersDbService {
  public class UsersDbService : IUsersDbService {

    private readonly MotorParkDbContext DbContext;

    public UsersDbService(MotorParkDbContext context) {
      DbContext = context;
    }

    public async Task<User?> getUserById(string Id) {
      var UserDTO = await DbContext.Users.FindAsync(Id);
      if(UserDTO == null) return null;
      return new User { Id = UserDTO.Id, Email = UserDTO.Email, Username = UserDTO.Username, Password = UserDTO.Password };
    }

    public async Task<AuthResponse> RegisterUser(User User) {
      string repeatedFields = "";
      var userWithSameEmail = await DbContext.Users.FirstOrDefaultAsync(user => user.Email == User.Email);
      var userWithSameUsername = await DbContext.Users.FirstOrDefaultAsync(user => user.Username == User.Username);
      if(userWithSameEmail != null) repeatedFields += "Email\n";
      if(userWithSameUsername != null) repeatedFields += "Username";
      if(repeatedFields != "") return new AuthResponse { LoggedUser = null, repeatedFields = repeatedFields };

      var UserDTO = new Entities.User {
        Id = Guid.NewGuid().ToString(),
        Username = User.Username,
        Email = User.Email,
        Password = User.Password,
      };

      DbContext.Add(UserDTO);
      await DbContext.SaveChangesAsync();
      var LoggedUser = new User { Id = UserDTO.Id, Username = UserDTO.Username, Email = UserDTO.Email, Password = UserDTO.Password };
      return new AuthResponse { LoggedUser = LoggedUser, repeatedFields = null };
    }

    public async Task<User?> ConfirmLogin(string Email, string Password) {
      var UserDTO = await DbContext.Users.FirstOrDefaultAsync(user => user.Email == Email && user.Password == Password);
      if(UserDTO == null) return null;
      else {
        var LoggedUser = new User { Id = UserDTO.Id, Username = UserDTO.Username, Email = Email, Password = Password };
        return LoggedUser;
      }
    }

    public async Task<AuthResponse> EditUser(User User) {
      if(User.Id == null) throw new Exception("User id must be provided");
      var repeatedFields = "";
      
      if(await DbContext.Users.FirstOrDefaultAsync(UserOnDb => UserOnDb.Id != User.Id && UserOnDb.Email == User.Email) != null)
        repeatedFields += "Email\n";
      if(await DbContext.Users.FirstOrDefaultAsync(UserOnDb => UserOnDb.Id != User.Id && UserOnDb.Username == User.Username) != null) 
        repeatedFields += "Username";
      if(repeatedFields != "") return new AuthResponse { LoggedUser = null, repeatedFields = repeatedFields };

      DbContext.Users.Update(new Entities.User {
        Id = User.Id,
        Username = User.Username,
        Email = User.Email,
        Password = User.Password
      });

      await DbContext.SaveChangesAsync();
      return new AuthResponse { LoggedUser = User, repeatedFields = null };
    }

    public async Task DeleteUser(string UserId) {
      var UserToDelete = await DbContext.Users.FindAsync(UserId);
      if(UserToDelete == null) return;
      DbContext.Users.Remove(UserToDelete);
      await DbContext.SaveChangesAsync();
    }
  }
}
