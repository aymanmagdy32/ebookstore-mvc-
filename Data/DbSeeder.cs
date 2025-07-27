using BookShoppingCartMvcUI.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookShoppingCartMvcUI.Data
{
    public class DbSeeder
    {


        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            var userMgr = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); 
            var roleMgr = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString())); 
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var admin = new IdentityUser
            {
                UserName = "sayadana123@gmail.com",
                Email = "sayadana123@gmail.com",
                EmailConfirmed = true ,
            };


            var userinDb = await userMgr.FindByEmailAsync(admin.Email);

            if (userinDb == null) {
                await userMgr.CreateAsync(admin, "27641556#Ayman"); 
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            
            }

        }
    }
}
