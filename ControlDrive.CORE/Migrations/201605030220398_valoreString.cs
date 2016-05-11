namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class valoreString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Valor", "cierre", c => c.String());
            AlterColumn("dbo.Valor", "ruta", c => c.String());
            AlterColumn("dbo.Valor", "conductor", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Valor", "conductor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Valor", "ruta", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Valor", "cierre", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
