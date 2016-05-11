namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class valorDocumentos1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documento", name: "TerceroId", newName: "ClienteId");
            RenameIndex(table: "dbo.Documento", name: "IX_TerceroId", newName: "IX_ClienteId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Documento", name: "IX_ClienteId", newName: "IX_TerceroId");
            RenameColumn(table: "dbo.Documento", name: "ClienteId", newName: "TerceroId");
        }
    }
}
