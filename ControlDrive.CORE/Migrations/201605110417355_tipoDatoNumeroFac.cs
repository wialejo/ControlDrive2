namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tipoDatoNumeroFac : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documento", "Numero", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documento", "Numero", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
