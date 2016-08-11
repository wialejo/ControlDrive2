namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ciudadesConSucursales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ciudad", "SucursalId", c => c.Int());
            CreateIndex("dbo.Ciudad", "SucursalId");
            AddForeignKey("dbo.Ciudad", "SucursalId", "dbo.Sucursal", "Id");
            Sql(@"
                    UPDATE Ciudad
                    SET SucursalId = 1
                ");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ciudad", "SucursalId", "dbo.Sucursal");
            DropIndex("dbo.Ciudad", new[] { "SucursalId" });
            DropColumn("dbo.Ciudad", "SucursalId");
        }
    }
}
