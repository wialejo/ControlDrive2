namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class sucursales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sucursal",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Descripcion = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ApplicationUserSucursal",
                c => new
                {
                    ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    Sucursal_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Sucursal_Id })
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sucursal", t => t.Sucursal_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Sucursal_Id);

            AddColumn("dbo.Conductor", "SucursalId", c => c.Int());
            AddColumn("dbo.Servicio", "SucursalId", c => c.Int());
            CreateIndex("dbo.Conductor", "SucursalId");
            CreateIndex("dbo.Servicio", "SucursalId");
            AddForeignKey("dbo.Conductor", "SucursalId", "dbo.Sucursal", "Id");
            AddForeignKey("dbo.Servicio", "SucursalId", "dbo.Sucursal", "Id");

         
        }

        public override void Down()
        {
            DropForeignKey("dbo.Servicio", "SucursalId", "dbo.Sucursal");
            DropForeignKey("dbo.Conductor", "SucursalId", "dbo.Sucursal");
            DropForeignKey("dbo.ApplicationUserSucursal", "Sucursal_Id", "dbo.Sucursal");
            DropForeignKey("dbo.ApplicationUserSucursal", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropIndex("dbo.ApplicationUserSucursal", new[] { "Sucursal_Id" });
            DropIndex("dbo.ApplicationUserSucursal", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Servicio", new[] { "SucursalId" });
            DropIndex("dbo.Conductor", new[] { "SucursalId" });
            DropColumn("dbo.Servicio", "SucursalId");
            DropColumn("dbo.Conductor", "SucursalId");
            DropTable("dbo.ApplicationUserSucursal");
            DropTable("dbo.Sucursal");
        }
    }
}
