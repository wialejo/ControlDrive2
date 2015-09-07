namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ControlDrive.API.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\ApplicationDbContext";
        }

        protected override void Seed(ControlDrive.API.Models.ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "admi",
                Email = "taiseer.joudeh@mymail.com",
                EmailConfirmed = true,
                PhoneNumber = "3112160076",
                PhoneNumberConfirmed = true


                //FirstName = "Taiseer",
                //LastName = "Joudeh",
                //Level = 1,
                //JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "admin");
        

        var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("admin");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "admin",
                    Email = "wi_alejo@hotmail.com",
                    PasswordHash = password,
                    PhoneNumber = "3175104254",
                    SecurityStamp = Guid.NewGuid().ToString()

                });
        }
    }
}
