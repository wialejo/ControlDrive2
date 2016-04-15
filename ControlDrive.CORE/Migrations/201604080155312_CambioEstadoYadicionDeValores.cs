namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioEstadoYadicionDeValores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Valor",
                c => new
                    {
                        ServicioId = c.Int(nullable: false),
                        cierre = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ruta = c.Decimal(nullable: false, precision: 18, scale: 2),
                        conductor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServicioId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .Index(t => t.ServicioId);
            
            AddColumn("dbo.Estado", "EnOperacion", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Valor", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.Valor", new[] { "ServicioId" });
            DropColumn("dbo.Estado", "EnOperacion");
            DropTable("dbo.Valor");
        }
    }
}
