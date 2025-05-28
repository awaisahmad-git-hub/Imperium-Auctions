using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrestigeAuction.Data;
using PrestigeAuction.Models;
using PrestigeAuction.Utility;

namespace PrestigeAuction.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public void Initialize()
        {
            // migrations if they are not applied
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex) { }
            // create roles if they are not created
            if (!_roleManager.RoleExistsAsync(StaticValues.Role_User).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticValues.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticValues.Role_Admin)).GetAwaiter().GetResult();

                // if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName="admin123@gmail.com",
                    Email= "admin123@gmail.com",
                    Name="Admin",
                    PhoneNumber="111122223333",
                    Address="address 123",
                    PostalCode="1234"
                }, "Admin123").GetAwaiter().GetResult();
                ApplicationUser? user = _context.ApplicationUsers.FirstOrDefault(u=>u.Email== "admin123@gmail.com");
                _userManager.AddToRoleAsync(user, StaticValues.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
