namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class valorDocumentos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Empresa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Nit = c.String(),
                        Ciudad = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documento", "Valor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Documento", "TerceroId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documento", "TerceroId");
            AddForeignKey("dbo.Documento", "TerceroId", "dbo.Aseguradora", "Id", cascadeDelete: false);
            DropColumn("dbo.Documento", "Tercero");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documento", "Tercero", c => c.String());
            DropForeignKey("dbo.Documento", "TerceroId", "dbo.Aseguradora");
            DropIndex("dbo.Documento", new[] { "TerceroId" });
            DropColumn("dbo.Documento", "TerceroId");
            DropColumn("dbo.Documento", "Valor");
            DropTable("dbo.Empresa");
        }
    }
}
