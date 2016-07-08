namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idTipoServicio : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Servicio", name: "TipoServicio_Id", newName: "TipoServicioId");
            RenameIndex(table: "dbo.Servicio", name: "IX_TipoServicio_Id", newName: "IX_TipoServicioId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Servicio", name: "IX_TipoServicioId", newName: "IX_TipoServicio_Id");
            RenameColumn(table: "dbo.Servicio", name: "TipoServicioId", newName: "TipoServicio_Id");
        }
    }
}
