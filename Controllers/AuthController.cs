using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_SegInfo.Services.IServices;

namespace WebApi_SegInfo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioServices usuarioServices, IConfiguration configuration)
        {
            _usuarioServices = usuarioServices;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioServices.Authenticate(request.UserName, request.Password);
            if (usuario == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas" });
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                   new Claim(ClaimTypes.NameIdentifier, usuario.PkUsuario.ToString()),
                   new Claim(ClaimTypes.Name, usuario.UserName)
               };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
