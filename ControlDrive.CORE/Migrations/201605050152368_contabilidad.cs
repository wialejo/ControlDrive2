namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contabilidad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Valor", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.Valor", new[] { "ServicioId" });
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(),
                        FechaRegistro = c.DateTime(nullable: false),
                        UsuarioRegistro_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.UsuarioRegistro_Id)
                .Index(t => t.UsuarioRegistro_Id);
            
            CreateTable(
                "dbo.Movimiento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServicioId = c.Int(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ConceptoCodigo = c.String(maxLength: 128),
                        ProveedorId = c.Int(),
                        ClienteId = c.Int(),
                        DocumentoId = c.Int(),
                        FechaRegistro = c.DateTime(),
                        UsuarioRegistroId = c.String(maxLength: 128),
                        FechaModificacion = c.DateTime(),
                        UsuarioModificacionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Aseguradora", t => t.ClienteId)
                .ForeignKey("dbo.ServicioConcepto", t => t.ConceptoCodigo)
                .ForeignKey("dbo.Documento", t => t.DocumentoId, cascadeDelete: true)
                .ForeignKey("dbo.Conductor", t => t.ProveedorId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUser", t => t.UsuarioModificacionId)
                .ForeignKey("dbo.ApplicationUser", t => t.UsuarioRegistroId)
                .Index(t => t.ServicioId)
                .Index(t => t.ConceptoCodigo)
                .Index(t => t.ProveedorId)
                .Index(t => t.ClienteId)
                .Index(t => t.DocumentoId)
                .Index(t => t.UsuarioRegistroId)
                .Index(t => t.UsuarioModificacionId);
            
            CreateTable(
                "dbo.ServicioConcepto",
                c => new
                    {
                        Codigo = c.String(nullable: false, maxLength: 128),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Codigo);
            
            DropColumn("dbo.Servicio", "NoFactura");
            Sql(@" 

                    insert into ServicioConcepto(Codigo,Descripcion )
                    SELECT 'PROVE_COND_ELE', 'Conductor elegido.'
                    UNION 
                    SELECT 'PROVE_RUTA_COND_ELE', 'Ruta de conductor elegido.'
                    UNION
                    SELECT 'CLIENTE_COND_ELE', 'Servicio de conductor elegido.'

                    INSERT INTO movimiento(ServicioId, Valor, ConceptoCodigo, ProveedorId, ClienteId)
                    select servicioId, CAST(replace(conductor,'.00','') AS decimal(10,2)) as valor, 'PROVE_COND_ELE' as concepto, conductorId as proveedor, null as cliente
                    from dbo.valor inner join servicio on servicioId = Id
                    where conductor <> ''
                    union all
                    select servicioId, CAST(replace(ruta,'.00','') AS decimal(10,2)), 'PROVE_RUTA_COND_ELE', rutaid, null
                    from dbo.valor inner join servicio on servicioId = Id
                    where ruta <> ''
                    union all
                    select servicioId, CAST(replace(cierre,'.00','') AS decimal(10,2)) , 'CLIENTE_COND_ELE', null, aseguradoraId
                    from dbo.valor inner join servicio on servicioId = Id
                    where cierre <> ''
                    "
              );
            DropTable("dbo.Valor");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Valor",
                c => new
                    {
                        ServicioId = c.Int(nullable: false),
                        cierre = c.String(),
                        ruta = c.String(),
                        conductor = c.String(),
                    })
                .PrimaryKey(t => t.ServicioId);
            
            AddColumn("dbo.Servicio", "NoFactura", c => c.String());
            DropForeignKey("dbo.Movimiento", "UsuarioRegistroId", "dbo.ApplicationUser");
            DropForeignKey("dbo.Movimiento", "UsuarioModificacionId", "dbo.ApplicationUser");
            DropForeignKey("dbo.Movimiento", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Movimiento", "ProveedorId", "dbo.Conductor");
            DropForeignKey("dbo.Movimiento", "DocumentoId", "dbo.Documento");
            DropForeignKey("dbo.Movimiento", "ConceptoCodigo", "dbo.ServicioConcepto");
            DropForeignKey("dbo.Movimiento", "ClienteId", "dbo.Aseguradora");
            DropForeignKey("dbo.Documento", "UsuarioRegistro_Id", "dbo.ApplicationUser");
            DropIndex("dbo.Movimiento", new[] { "UsuarioModificacionId" });
            DropIndex("dbo.Movimiento", new[] { "UsuarioRegistroId" });
            DropIndex("dbo.Movimiento", new[] { "DocumentoId" });
            DropIndex("dbo.Movimiento", new[] { "ClienteId" });
            DropIndex("dbo.Movimiento", new[] { "ProveedorId" });
            DropIndex("dbo.Movimiento", new[] { "ConceptoCodigo" });
            DropIndex("dbo.Movimiento", new[] { "ServicioId" });
            DropIndex("dbo.Documento", new[] { "UsuarioRegistro_Id" });
            DropTable("dbo.ServicioConcepto");
            DropTable("dbo.Movimiento");
            DropTable("dbo.Documento");
            CreateIndex("dbo.Valor", "ServicioId");
            AddForeignKey("dbo.Valor", "ServicioId", "dbo.Servicio", "Id");
        }
    }
}
