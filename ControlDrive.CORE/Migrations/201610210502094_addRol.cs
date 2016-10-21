namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUser", "Rol", c => c.String(nullable: false, defaultValueSql: "'Administrador'"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUser", "Rol");
        }
    }
}
