namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTiposConceptos : DbMigration
    {
        public override void Up()
        {
            Sql(@"
               
                IF NOT EXISTS (SELECT * FROM ServicioConcepto WHERE Codigo = 'PROVE_COORDINACION') BEGIN
	                INSERT INTO ServicioConcepto(Codigo,Descripcion)
	                SELECT 'PROVE_COORDINACION', 'Coordinación'
                END

                if NOT EXISTS (select * from ServicioConcepto where Codigo = 'PROVE_MENSAJERIA') BEGIN
	                INSERT INTO ServicioConcepto(Codigo,Descripcion  )
	                SELECT 'PROVE_MENSAJERIA', 'Mensajería'
                END

                if NOT EXISTS (select * from ServicioConcepto where Codigo = 'CLIENTE_MENSAJERIA') BEGIN
	                INSERT INTO ServicioConcepto(Codigo,Descripcion , TipoConcepto )
	                SELECT 'CLIENTE_MENSAJERIA', 'Mensajería', 1
                END

                if NOT EXISTS (select * from ServicioConcepto where Codigo = 'CLIENTE_VALET') BEGIN
	                INSERT INTO ServicioConcepto(Codigo,Descripcion, TipoConcepto)
	                SELECT 'CLIENTE_VALET', 'Valet parking', 1
                END

                if NOT EXISTS (select * from ServicioConcepto where Codigo = 'PROVE_VALET') BEGIN
	                INSERT INTO ServicioConcepto(Codigo,Descripcion )
	                SELECT 'PROVE_VALET', 'Valet parking'
                END

				UPDATE	ServicioConcepto
				SET  TipoProveedor = 1
				WHERE Codigo = 'PROVE_RUTA_COND_ELE'	");
        }
        
        public override void Down()
        {
            Sql(@"
				 
                DELETE ServicioConcepto
                WHERE Codigo IN ('PROVE_COORDINACION','PROVE_MENSAJERIA','CLIENTE_MENSAJERIA','CLIENTE_VALET','PROVE_VALET')
                ");
        }
    }
}
