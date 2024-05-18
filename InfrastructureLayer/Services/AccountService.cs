using DomainLayer.DTOs;
using DomainLayer.Responses;
using InfrastructureLayer.Identity;
using Microsoft.AspNetCore.Identity;

namespace InfrastructureLayer.Services
{
    public class AccountService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        ITokenService tokenService) : IAccountService
    {
        public async Task<LoginResponse> LoginUser(LoginDTO loginDTO)
        {
            // Checks if model is empty.
            if (loginDTO == null) return new LoginResponse(false, "Model is empty", null!);

            // Checks if user exists.
            var getUser = await userManager.FindByEmailAsync(loginDTO.Email!);
            if (getUser == null) return new LoginResponse(false, "User not found", null!);

            // Checks if password is correct.
            bool checkUserPassword = await userManager.CheckPasswordAsync(getUser, loginDTO.Password!);
            if (!checkUserPassword) return new LoginResponse(false, "Invalid password", null!);

            // Checks the role of the user.
            var getUserRole = await userManager.GetRolesAsync(getUser);

            // Pass model to token service.
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = tokenService.CreateToken(userSession);

            return new LoginResponse(true, "Login Successful", token);
        }

        public async Task<GeneralResponse> RegisterUser(RegisterDTO registerDTO)
        {
            // Checks if model is empty
            if (registerDTO == null) return new GeneralResponse(false, "Model is empty");

            // Check if user exists
            var checkUser = await userManager.FindByEmailAsync(registerDTO.Email!);
            if (checkUser != null) return new GeneralResponse(false, "User already registered");

            // Save user
            var newUser = new ApplicationUser()
            {
                Name = registerDTO.Name,
                Email = registerDTO.Email,
                PasswordHash = registerDTO.Password,
                UserName = registerDTO.Email
            };

            // Create user
            var createUser = await userManager.CreateAsync(newUser, registerDTO.Password!);
            if (!createUser.Succeeded) return new GeneralResponse(false, "Error! Please try again");

            // Create and assign role to user/admin
            var checkAdminRole = await roleManager.FindByNameAsync("Admin");
            if (checkAdminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Admin account created");
            }
            else
            {
                var checkUserRole = await roleManager.FindByNameAsync("User");
                if (checkUserRole == null) await roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                await userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(true, "User account created");
            }

        }


    }
}
