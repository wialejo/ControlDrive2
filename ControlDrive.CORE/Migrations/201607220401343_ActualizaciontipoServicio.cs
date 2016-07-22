namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizaciontipoServicio : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    UPDATE servicio
                    SET tipoServicioId = 1
                ");
        }
        
        public override void Down()
        {
        }
    }
}
