namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MovimientosProveedorCliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movimiento", "ProveedorId", c => c.Int());
            AddColumn("dbo.Movimiento", "ClienteId", c => c.Int());
            CreateIndex("dbo.Movimiento", "ProveedorId");
            CreateIndex("dbo.Movimiento", "ClienteId");
            AddForeignKey("dbo.Movimiento", "ClienteId", "dbo.Aseguradora", "Id");
            AddForeignKey("dbo.Movimiento", "ProveedorId", "dbo.Conductor", "Id");
            Sql(@"
                    UPDATE M
                    SET M.ProveedorId =  S.ConductorId
                    FROM	Servicio S 
		                    INNER JOIN Movimiento M ON M.ServicioId = S.Id
                    WHERE ConceptoCodigo = 'PROVE_COND_ELE'


                    UPDATE M
                    SET M.ProveedorId = S.RutaId
                    FROM	Servicio S 
		                    INNER JOIN Movimiento M ON M.ServicioId = S.Id
                    WHERE ConceptoCodigo = 'PROVE_RUTA_COND_ELE'

                    UPDATE M
                    SET M.ClienteId = S.AseguradoraId
                    FROM	Servicio S 
		                    INNER JOIN Movimiento M ON M.ServicioId = S.Id
                    WHERE ConceptoCodigo = 'CLIENTE_COND_ELE'

                ");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Movimiento", "ProveedorId", "dbo.Conductor");
            DropForeignKey("dbo.Movimiento", "ClienteId", "dbo.Aseguradora");
            DropIndex("dbo.Movimiento", new[] { "ClienteId" });
            DropIndex("dbo.Movimiento", new[] { "ProveedorId" });
            DropColumn("dbo.Movimiento", "ClienteId");
            DropColumn("dbo.Movimiento", "ProveedorId");
        }
    }
}
