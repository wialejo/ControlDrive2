namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addServicioNotificacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servicio", "Notificado", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servicio", "Notificado");
        }
    }
}
