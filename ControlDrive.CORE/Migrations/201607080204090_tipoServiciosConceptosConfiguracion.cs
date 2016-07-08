namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class tipoServiciosConceptosConfiguracion : DbMigration
    {
        public override void Up()
        {
            Sql(@"  IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'CLIENTE_COND_ELE' AND TipoServicio_Id = 1) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'CLIENTE_COND_ELE' as concepto, 1 as tipoServicio
                    END
                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_COND_ELE' AND TipoServicio_Id = 1) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'PROVE_COND_ELE' as concepto, 1 as tipoServicio
                    END
                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_RUTA_COND_ELE' AND TipoServicio_Id = 1) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'PROVE_RUTA_COND_ELE' as concepto, 1 as tipoServicio
                    END


                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_COORDINACION' AND TipoServicio_Id = 2) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'PROVE_COORDINACION' as concepto, 2 as tipoServicio
                    END

                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'CLIENTE_VALET' AND TipoServicio_Id = 3) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'CLIENTE_VALET' as concepto, 3 as tipoServicio
                    END
                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_VALET' AND TipoServicio_Id = 3) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'PROVE_VALET' as concepto, 3 as tipoServicio
                    END
                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_VALET' AND TipoServicio_Id = 6) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'PROVE_VALET' as concepto, 6 as tipoServicio
                    END

                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'CLIENTE_MENSAJERIA' AND TipoServicio_Id = 4) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'CLIENTE_MENSAJERIA' as concepto, 4 as tipoServicio
                    END
                    IF NOT EXISTS (SELECT * FROM ConceptoPagoTipoServicio WHERE ConceptoPago_Codigo = 'PROVE_MENSAJERIA' AND TipoServicio_Id = 4) BEGIN
	                    INSERT INTO ConceptoPagoTipoServicio
	                    SELECT 'PROVE_MENSAJERIA' as concepto, 4 as tipoServicio
                    END


                    ");
        }

        public override void Down()
        {
            Sql(@"
                DELETE ConceptoPagoTipoServicio
                ");
        }
    }
}
