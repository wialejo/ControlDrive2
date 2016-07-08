namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tipoProveedor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioConcepto", "TipoProveedor", c => c.Int(nullable: false, defaultValue: 0));

        }
        
        public override void Down()
        {
            DropColumn("dbo.ServicioConcepto", "TipoProveedor");
        }
    }
}
