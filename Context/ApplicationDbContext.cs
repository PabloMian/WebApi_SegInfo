using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi_SegInfo.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //las clases que se van a mappear en la BD

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Insertar en tabla Usuario
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    PkUsuario = 1,
                    Nombre = "Majo",
                    UserName = "Usuario",
                    Password = "123",
                    FkRol = 1 // Aqui debes poner rol correspondiente

                });


            //Insertar en la tabla Roles

            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    PkRol = 1,
                    Nombre = "sa"
                });

        }
    }
}
