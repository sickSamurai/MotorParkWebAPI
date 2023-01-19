using MotorPark.Models;

namespace MotorPark.Services.UsersDbService
{
    public interface IUsersDbService
    {
        Task<User?> getUserById(string Id);

        Task<AuthResponse> RegisterUser(User User);

        Task<User?> ConfirmLogin(string Email, string Password);

        Task<AuthResponse> EditUser(User User);

        Task DeleteUser(string id);
    }
}
