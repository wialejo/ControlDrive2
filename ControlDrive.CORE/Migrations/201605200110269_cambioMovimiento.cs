namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioMovimiento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movimiento", "ClienteId", "dbo.Aseguradora");
            DropForeignKey("dbo.Movimiento", "ProveedorId", "dbo.Conductor");
            DropIndex("dbo.Movimiento", new[] { "ProveedorId" });
            DropIndex("dbo.Movimiento", new[] { "ClienteId" });
            AddColumn("dbo.ServicioConcepto", "TipoConcepto", c => c.Int(nullable: false));
            DropColumn("dbo.Movimiento", "ProveedorId");
            DropColumn("dbo.Movimiento", "ClienteId");
            Sql(@" 
                    UPDATE dbo.ServicioConcepto
                    SET TipoConcepto = CASE CODIGO  WHEN 'CLIENTE_COND_ELE' THEN 1 ELSE 0 END
                "
             );
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movimiento", "ClienteId", c => c.Int());
            AddColumn("dbo.Movimiento", "ProveedorId", c => c.Int());
            DropColumn("dbo.ServicioConcepto", "TipoConcepto");
            CreateIndex("dbo.Movimiento", "ClienteId");
            CreateIndex("dbo.Movimiento", "ProveedorId");
            AddForeignKey("dbo.Movimiento", "ProveedorId", "dbo.Conductor", "Id");
            AddForeignKey("dbo.Movimiento", "ClienteId", "dbo.Aseguradora", "Id");
        }
    }
}
