namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movimientoAprobado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movimiento", "Aprobado", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movimiento", "Aprobado");
        }
    }
}
