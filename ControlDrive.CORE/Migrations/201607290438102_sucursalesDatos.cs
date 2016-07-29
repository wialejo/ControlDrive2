namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sucursalesDatos : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    IF NOT EXISTS(SELECT * FROM [dbo].[Sucursal] WHERE ID = 1) BEGIN
	                    SET IDENTITY_INSERT SUCURSAL ON
	                    INSERT INTO SUCURSAL(id, Descripcion)
	                    SELECT 1, 'Principal'
	                    SET IDENTITY_INSERT SUCURSAL OFF
                    END


                    UPDATE Servicio
                    SET SucursalId = 1

                    INSERT INTO ApplicationUserSucursal(ApplicationUser_Id, Sucursal_Id)
                    SELECT U.Id, 1
                    FROM ApplicationUser U
	                    LEFT JOIN ApplicationUserSucursal S ON U.ID = S.ApplicationUser_Id AND S.Sucursal_Id = 1
                    WHERE S.ApplicationUser_Id IS NULL

                    UPDATE conductor
                    SET SucursalId = 1

                ");
        }
        
        public override void Down()
        {
        }
    }
}
