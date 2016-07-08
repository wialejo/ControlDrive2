namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tiposServicio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servicio", "TipoServicio", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Servicio", "Observacion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servicio", "Observacion");
            DropColumn("dbo.Servicio", "TipoServicio");
        }
    }
}
