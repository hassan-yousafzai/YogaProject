namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToSessionScheduler : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionSchedulers", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionSchedulers", "UserId");
        }
    }
}
