using Domain.DTO;
using Domain.Entities;

namespace WebApi_SegInfo.Services.IServices
{
    public interface IUsuarioServices
    {

        /// Obtiene la lista completa de usuarios con sus roles asociados.

        Task<Response<List<Usuario>>> GetAll();


        /// Obtiene un usuario por su ID.

        Task<Response<Usuario>> GetbyId(int id);


        /// Crea un nuevo usuario.

        Task<Response<Usuario>> Create(UsuarioRequest request);


        /// Actualiza un usuario existente.

        Task<Response<Usuario>> Update(int id, UsuarioRequest request);

        /// Elimina un usuario por su ID.

        Task<Response<bool>> Delete(int id);
    }
}
