namespace YogaFitnessClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateActivityModel : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Activities (Name) VALUES ('1 to 1 Yoga')");
            Sql("INSERT INTO Activities (Name) VALUES ('Yoga Laughter')");
            Sql("INSERT INTO Activities (Name) VALUES ('Yin Toga 2')");
            Sql("INSERT INTO Activities (Name) VALUES ('Beginners Yoga')");
        }
        
        public override void Down()
        {
        }
    }
}
