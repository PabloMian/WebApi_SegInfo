using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_SegInfo.Services.IServices;


namespace WebApi_SegInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // Requiere token JWT para todos los endpoints
    public class RolController : ControllerBase
    {
        private readonly IRolServices _rolServices;

        public RolController(IRolServices rolServices)
        {
            _rolServices = rolServices;
        }

        /// Obtiene la lista de todos los roles.
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _rolServices.GetAll();
            return Ok(response);
        }

        /// Obtiene un rol por su ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolById(int id)
        {
            var response = await _rolServices.GetById(id);
            return Ok(response);
        }

        /// Crea un nuevo rol.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string nombre)
        {
            var response = await _rolServices.Create(nombre);
            return Ok(response);
        }

        /// Actualiza un rol existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string nombre)
        {
            var response = await _rolServices.Update(id, nombre);
            return Ok(response);
        }

        /// Elimina un rol por su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _rolServices.Delete(id);
            return Ok(response);
        }
    }
}
