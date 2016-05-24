namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addrelacionestadosseguimientos : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                update dbo.Seguimiento 
                set NuevoEstado = 'RG'
                where NuevoEstado in ('EN','RC')
            ");
            AlterColumn("dbo.Seguimiento", "NuevoEstado", c => c.String(maxLength: 128));
            CreateIndex("dbo.Seguimiento", "NuevoEstado");
            AddForeignKey("dbo.Seguimiento", "NuevoEstado", "dbo.Estado", "Codigo");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Seguimiento", "NuevoEstado", "dbo.Estado");
            DropIndex("dbo.Seguimiento", new[] { "NuevoEstado" });
            AlterColumn("dbo.Seguimiento", "NuevoEstado", c => c.String());
        }
    }
}
