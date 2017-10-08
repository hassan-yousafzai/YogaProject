using YogaFitnessClub.Models;

namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<YogaFitnessClub.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(YogaFitnessClub.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Customers.AddOrUpdate(new Customer()
            {
                Address = "test",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "test@test.com",
                Id = 1,
                Name = "Hassan",
                Postcode = "Postcode"
            });

            context.Instructors.AddOrUpdate(new Instructor()
            {
                Address = "test",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "test@test.com",
                Id = 1,
                Name = "Hassan",
                Postcode = "Postcode",
                Gender = "Male",
                NiNumber = "NiNumber"
            });
        }
    }
}
