namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInstructorToModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Birthdate = c.DateTime(nullable: false),
                        Address = c.String(nullable: false),
                        Postcode = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        NiNumber = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Instructors");
        }
    }
}
