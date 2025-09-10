using Microsoft.AspNetCore.Identity;

namespace PetSitter.DataAccess.Services.Role;

public class RoleService
{
    public static async Task SeedRole(RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roleValues = "User";
        
        foreach (var roleValue in roleValues)
        {
            var roleName = roleValue.ToString().ToUpper();
            
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }
}