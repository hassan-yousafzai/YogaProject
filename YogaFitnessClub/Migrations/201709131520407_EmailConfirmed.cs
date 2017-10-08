namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailConfirmed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ConfirmedEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ConfirmedEmail");
        }
    }
}
