namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addproveedordocumentos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documento", "ClienteId", "dbo.Aseguradora");
            DropIndex("dbo.Documento", new[] { "ClienteId" });
            AddColumn("dbo.Documento", "ProveedorId", c => c.Int());
            AlterColumn("dbo.Documento", "ClienteId", c => c.Int());
            CreateIndex("dbo.Documento", "ClienteId");
            CreateIndex("dbo.Documento", "ProveedorId");
            AddForeignKey("dbo.Documento", "ProveedorId", "dbo.Conductor", "Id");
            AddForeignKey("dbo.Documento", "ClienteId", "dbo.Aseguradora", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documento", "ClienteId", "dbo.Aseguradora");
            DropForeignKey("dbo.Documento", "ProveedorId", "dbo.Conductor");
            DropIndex("dbo.Documento", new[] { "ProveedorId" });
            DropIndex("dbo.Documento", new[] { "ClienteId" });
            AlterColumn("dbo.Documento", "ClienteId", c => c.Int(nullable: false));
            DropColumn("dbo.Documento", "ProveedorId");
            CreateIndex("dbo.Documento", "ClienteId");
            AddForeignKey("dbo.Documento", "ClienteId", "dbo.Aseguradora", "Id", cascadeDelete: false);
        }
    }
}
