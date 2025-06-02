using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi_SegInfo.Context;
using WebApi_SegInfo.Services.IServices;

namespace WebApi_SegInfo.Services.IServices
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ApplicationDbContext _context;

        public UsuarioServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<Usuario>>> GetAll()
        {
            try
            {
                List<Usuario> response = await _context.Usuarios
                    .Include(x => x.Roles)
                    .ToListAsync();
                return new Response<List<Usuario>>(response, "Lista de usuarios obtenida correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener usuarios: {ex.Message}");
            }
        }

        public async Task<Response<Usuario>> GetbyId(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.PkUsuario == id);
                if (usuario == null)
                {
                    return new Response<Usuario>(null, "Usuario no encontrado");
                }
                return new Response<Usuario>(usuario, "Usuario encontrado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener usuario: {ex.Message}");
            }
        }

        public async Task<Response<Usuario>> Create(UsuarioRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                {
                    return new Response<Usuario>(null, "Nombre, UserName y Password son obligatorios");
                }

                if (request.FkRol.HasValue && !await _context.Roles.AnyAsync(r => r.PkRol == request.FkRol))
                {
                    return new Response<Usuario>(null, "El rol especificado no existe");
                }

                if (await _context.Usuarios.AnyAsync(u => u.UserName == request.UserName))
                {
                    return new Response<Usuario>(null, "El UserName ya está en uso");
                }

                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    UserName = request.UserName,
                    Password = request.Password, // Sin hashear
                    FkRol = request.FkRol
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return new Response<Usuario>(usuario, "Usuario creado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear usuario: {ex.Message}");
            }
        }

        public async Task<Response<Usuario>> Update(int id, UsuarioRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                {
                    return new Response<Usuario>(null, "Nombre, UserName y Password son obligatorios");
                }

                if (request.FkRol.HasValue && !await _context.Roles.AnyAsync(r => r.PkRol == request.FkRol))
                {
                    return new Response<Usuario>(null, "El rol especificado no existe");
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.PkUsuario == id);
                if (usuario == null)
                {
                    return new Response<Usuario>(null, "Usuario no encontrado");
                }

                if (await _context.Usuarios.AnyAsync(u => u.UserName == request.UserName && u.PkUsuario != id))
                {
                    return new Response<Usuario>(null, "El UserName ya está en uso");
                }

                usuario.Nombre = request.Nombre;
                usuario.UserName = request.UserName;
                usuario.Password = request.Password; // Sin hashear
                usuario.FkRol = request.FkRol;

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return new Response<Usuario>(usuario, "Usuario actualizado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar usuario: {ex.Message}");
            }
        }

        public async Task<Response<bool>> Delete(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.PkUsuario == id);
                if (usuario == null)
                {
                    return new Response<bool>(false, "Usuario no encontrado");
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return new Response<bool>(true, "Usuario eliminado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar usuario: {ex.Message}");
            }
        }

        public async Task<Usuario> Authenticate(string username, string password)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

            return usuario; // Devuelve null si las credenciales son inválidas
        }
    }
}