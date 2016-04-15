namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNoFactura : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servicio", "NoFactura", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servicio", "NoFactura");
        }
    }
}
