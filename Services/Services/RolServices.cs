using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi_SegInfo.Context;
using WebApi_SegInfo.Services.IServices;

namespace WebApi_SegInfo.Services.Services
{
    public class RolServices : IRolServices
    {
        private readonly ApplicationDbContext _context;

        public RolServices(ApplicationDbContext context)
        {
            _context = context;
        }
        /// Obtiene la lista completa de roles.
        public async Task<Response<List<Rol>>> GetAll()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                return new Response<List<Rol>>(roles, "Lista de roles obtenida correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener roles: {ex.Message}");
            }
        }

        /// Obtiene un rol por su ID.

        public async Task<Response<Rol>> GetById(int id)
        {
            try
            {
                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.PkRol == id);
                if (rol == null)
                {
                    return new Response<Rol>(null, "Rol no encontrado");
                }
                return new Response<Rol>(rol, "Rol encontrado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rol: {ex.Message}");
            }
        }


        /// Crea un nuevo rol.

        public async Task<Response<Rol>> Create(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return new Response<Rol>(null, "El nombre del rol es obligatorio");
                }

                if (await _context.Roles.AnyAsync(r => r.Nombre == nombre))
                {
                    return new Response<Rol>(null, "El nombre del rol ya existe");
                }

                var rol = new Rol { Nombre = nombre };
                _context.Roles.Add(rol);
                await _context.SaveChangesAsync();
                return new Response<Rol>(rol, "Rol creado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear rol: {ex.Message}");
            }
        }

        /// Actualiza un rol existente.

        public async Task<Response<Rol>> Update(int id, string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return new Response<Rol>(null, "El nombre del rol es obligatorio");
                }

                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.PkRol == id);
                if (rol == null)
                {
                    return new Response<Rol>(null, "Rol no encontrado");
                }

                if (await _context.Roles.AnyAsync(r => r.Nombre == nombre && r.PkRol != id))
                {
                    return new Response<Rol>(null, "El nombre del rol ya existe");
                }

                rol.Nombre = nombre;
                _context.Roles.Update(rol);
                await _context.SaveChangesAsync();
                return new Response<Rol>(rol, "Rol actualizado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar rol: {ex.Message}");
            }
        }

        /// Elimina un rol por su ID.

        public async Task<Response<bool>> Delete(int id)
        {
            try
            {
                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.PkRol == id);
                if (rol == null)
                {
                    return new Response<bool>(false, "Rol no encontrado");
                }

                if (await _context.Usuarios.AnyAsync(u => u.FkRol == id))
                {
                    return new Response<bool>(false, "No se puede eliminar el rol porque está asignado a uno o más usuarios");
                }

                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
                return new Response<bool>(true, "Rol eliminado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar rol: {ex.Message}");
            }
        }
    }
}
