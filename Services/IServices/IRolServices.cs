using Domain.Entities;

namespace WebApi_SegInfo.Services.IServices
{
    public interface IRolServices
    {

        /// Obtiene la lista completa de roles.

        Task<Response<List<Rol>>> GetAll();


        /// Obtiene un rol por su ID.

        Task<Response<Rol>> GetById(int id);


        /// Crea un nuevo rol.

        Task<Response<Rol>> Create(string nombre);


        /// Actualiza un rol existente.

        Task<Response<Rol>> Update(int id, string nombre);


        /// Elimina un rol por su ID.

        Task<Response<bool>> Delete(int id);
    }
}
