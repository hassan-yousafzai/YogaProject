namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSessionScheduler1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionSchedulers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        TutorName = c.String(nullable: false),
                        Description = c.String(maxLength: 255),
                        Title = c.String(nullable: false, maxLength: 100),
                        ThemeColour = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionSchedulers");
        }
    }
}
