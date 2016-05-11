namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentoContable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documento", name: "UsuarioRegistro_Id", newName: "UsuarioRegistroId");
            RenameIndex(table: "dbo.Documento", name: "IX_UsuarioRegistro_Id", newName: "IX_UsuarioRegistroId");
            AddColumn("dbo.Documento", "Numero", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Documento", "Fecha", c => c.DateTime(nullable: false));
            AddColumn("dbo.Documento", "Concepto", c => c.String());
            AddColumn("dbo.Documento", "Tercero", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documento", "Tercero");
            DropColumn("dbo.Documento", "Concepto");
            DropColumn("dbo.Documento", "Fecha");
            DropColumn("dbo.Documento", "Numero");
            RenameIndex(table: "dbo.Documento", name: "IX_UsuarioRegistroId", newName: "IX_UsuarioRegistro_Id");
            RenameColumn(table: "dbo.Documento", name: "UsuarioRegistroId", newName: "UsuarioRegistro_Id");
        }
    }
}
