using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApi_SegInfo.Services.IServices;

namespace WebApi_SegInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        /// Obtiene la lista de todos los usuarios.
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usuarioServices.GetAll();
            return Ok(response);
        }

        /// Obtiene un usuario por su ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _usuarioServices.GetbyId(id);
            return Ok(response);
        }

        /// Crea un nuevo usuario.
        [HttpPost]
        public async Task<IActionResult> Create(UsuarioRequest request)
        {
            var response = await _usuarioServices.Create(request);
            return Ok(response);
        }

        /// Actualiza un usuario existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioRequest request)
        {
            var response = await _usuarioServices.Update(id, request);
            return Ok(response);
        }

        /// Elimina un usuario por su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _usuarioServices.Delete(id);
            return Ok(response);
        }
    }
}
