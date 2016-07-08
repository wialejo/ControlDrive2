namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTipoServicio : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ServicioConcepto", newName: "ConceptoPago");
            CreateTable(
                "dbo.TipoServicio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        RequiereSeguimiento = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConceptoPagoTipoServicio",
                c => new
                    {
                        ConceptoPago_Codigo = c.String(nullable: false, maxLength: 128),
                        TipoServicio_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ConceptoPago_Codigo, t.TipoServicio_Id })
                .ForeignKey("dbo.ConceptoPago", t => t.ConceptoPago_Codigo, cascadeDelete: true)
                .ForeignKey("dbo.TipoServicio", t => t.TipoServicio_Id, cascadeDelete: true)
                .Index(t => t.ConceptoPago_Codigo)
                .Index(t => t.TipoServicio_Id);
            
            AddColumn("dbo.Servicio", "TipoServicio_Id", c => c.Int());
            CreateIndex("dbo.Servicio", "TipoServicio_Id");
            AddForeignKey("dbo.Servicio", "TipoServicio_Id", "dbo.TipoServicio", "Id");
            DropColumn("dbo.Servicio", "TipoServicio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servicio", "TipoServicio", c => c.Int(nullable: false));
            DropForeignKey("dbo.Servicio", "TipoServicio_Id", "dbo.TipoServicio");
            DropForeignKey("dbo.ConceptoPagoTipoServicio", "TipoServicio_Id", "dbo.TipoServicio");
            DropForeignKey("dbo.ConceptoPagoTipoServicio", "ConceptoPago_Codigo", "dbo.ConceptoPago");
            DropIndex("dbo.ConceptoPagoTipoServicio", new[] { "TipoServicio_Id" });
            DropIndex("dbo.ConceptoPagoTipoServicio", new[] { "ConceptoPago_Codigo" });
            DropIndex("dbo.Servicio", new[] { "TipoServicio_Id" });
            DropColumn("dbo.Servicio", "TipoServicio_Id");
            DropTable("dbo.ConceptoPagoTipoServicio");
            DropTable("dbo.TipoServicio");
            RenameTable(name: "dbo.ConceptoPago", newName: "ServicioConcepto");
        }
    }
}
