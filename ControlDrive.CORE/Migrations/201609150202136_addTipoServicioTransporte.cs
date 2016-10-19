namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTipoServicioTransporte : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                IF(NOT EXISTS(SELECT * FROM ConceptoPago WHERE Codigo = 'CLIENTE_TRANSPORTE')) BEGIN
                    INSERT INTO dbo.ConceptoPago(Codigo, Descripcion, TipoConcepto, TipoProveedor)
                    SELECT 'CLIENTE_TRANSPORTE', 'Transporte', 1, 0
                END
                IF(NOT EXISTS(SELECT * FROM ConceptoPago WHERE Codigo = 'PROVE_TRANSPORTE')) BEGIN
                    INSERT INTO dbo.ConceptoPago(Codigo, Descripcion, TipoConcepto, TipoProveedor)
                    SELECT 'PROVE_TRANSPORTE', 'Transporte', 0, 0
                END

                IF(NOT EXISTS(SELECT * FROM TipoServicio WHERE Descripcion = 'Transporte')) BEGIN
                    SET IDENTITY_INSERT TipoServicio ON
                    INSERT INTO[dbo].[TipoServicio]([Id],[Descripcion],[RequiereSeguimiento])
                    SELECT 7,'Transporte', 1
                    SET IDENTITY_INSERT TipoServicio OFF
                END

                IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'CLIENTE_TRANSPORTE' AND TipoServicio_Id = 7) BEGIN
                    INSERT INTO ConceptoPagoTipoServicio
                    SELECT 'CLIENTE_TRANSPORTE' as concepto, 7 as tipoServicio
                END
                IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_TRANSPORTE' AND TipoServicio_Id = 7) BEGIN
                    INSERT INTO ConceptoPagoTipoServicio
                    SELECT 'PROVE_TRANSPORTE' as concepto, 7 as tipoServicio
                END
                ");
        }
        
        public override void Down()
        {

        }
    }
}
