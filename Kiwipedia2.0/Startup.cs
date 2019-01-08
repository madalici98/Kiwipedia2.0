using Kiwipedia2._0.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kiwipedia2._0.Startup))]
namespace Kiwipedia2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // creez un cont de admin si rolurile
            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // stabilirea rolurilor aplicatiei
            // adaug rolul de administrator
            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);

                // cont de admin pentru Mada
                var user = new ApplicationUser();
                user.UserName = "mada";
                user.Email = "mada@admin.com";
                var adminCreated = UserManager.Create(user,"Mada");

                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Administrator");
                }

                // cont de admin pentru andrei
                user = new ApplicationUser();
                user.UserName = "andrei";
                user.Email = "andrei@admin.com";
                adminCreated = UserManager.Create(user, "Andrei");

                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Administrator");
                }
            }

            // adaug rolul de editor (moderator)
            if(!roleManager.RoleExists("Editor"))
            {
                var role = new IdentityRole();
                role.Name = "Editor";
                roleManager.Create(role);
            }

            // adaug rolul de utilizator (inregistrat)
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }

            // adaug rolul de vizitator (utilizator neinregistrat)
            if (!roleManager.RoleExists("Visitor"))
            {
                var role = new IdentityRole();
                role.Name = "Visitor";
                roleManager.Create(role);
            }
        }
    }
}
