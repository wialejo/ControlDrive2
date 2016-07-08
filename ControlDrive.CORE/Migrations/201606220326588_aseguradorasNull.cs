namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aseguradorasNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Servicio", "AseguradoraId", "dbo.Aseguradora");
            DropIndex("dbo.Servicio", new[] { "AseguradoraId" });
            AlterColumn("dbo.Servicio", "AseguradoraId", c => c.Int());
            CreateIndex("dbo.Servicio", "AseguradoraId");
            AddForeignKey("dbo.Servicio", "AseguradoraId", "dbo.Aseguradora", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servicio", "AseguradoraId", "dbo.Aseguradora");
            DropIndex("dbo.Servicio", new[] { "AseguradoraId" });
            AlterColumn("dbo.Servicio", "AseguradoraId", c => c.Int(nullable: false));
            CreateIndex("dbo.Servicio", "AseguradoraId");
            AddForeignKey("dbo.Servicio", "AseguradoraId", "dbo.Aseguradora", "Id", cascadeDelete: true);
        }
    }
}
