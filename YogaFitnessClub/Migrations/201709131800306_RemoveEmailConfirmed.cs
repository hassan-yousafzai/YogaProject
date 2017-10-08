namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmailConfirmed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ConfirmedEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ConfirmedEmail", c => c.Boolean(nullable: false));
        }
    }
}
