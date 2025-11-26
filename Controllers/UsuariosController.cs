using ulp_lab3_keiebo_servidor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ulp_lab3_keiebo_servidor.Controllers
{
	[Route("[Controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration configuracion;
		private string hashSalt = "";

		public UsuariosController(DataContext context, IConfiguration config)
		{
			contexto = context;
			configuracion = config;
			hashSalt = configuracion["Salt"] ?? "";
		}

		// POST: Usuarios/Login
		[HttpPost("Login")]
		[AllowAnonymous]
		public IActionResult Login([FromForm] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Password,
					salt: System.Text.Encoding.ASCII.GetBytes(hashSalt),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 10000,
					numBytesRequested: 256 / 8
				));

				var propietario = contexto.Usuarios.FirstOrDefault(x => x.Email == loginView.Email);
				if (propietario == null || hashed != propietario.Password)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					string secretKey = configuracion["TokenAuthentication:SecretKey"] ?? throw new ArgumentNullException(nameof(secretKey));
					var securityKey = secretKey != null ? new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)) : null;
					var credenciales = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, propietario.Email),
						new Claim("Id", propietario.Id.ToString())
					};

					var token = new JwtSecurityToken(
						issuer: configuracion["TokenAuthentication:Issuer"],
						audience: configuracion["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);

					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET: Popietarios/Perfil
		[HttpGet("Perfil")]
		[Authorize]
		public IActionResult GetPropietario()
		{
			try
			{
				var propietario = User.Identity != null
					? contexto.Usuarios
						.Where(x => x.Email == User.Identity.Name)
						.Select(x => new Propietario(x))
						.FirstOrDefault()
					: null;

				if (propietario == null)
				{
					return NotFound();
				}

				return Ok(propietario);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: Usuarios/Editar
		[HttpPut("Editar")]
		[Authorize]
		public IActionResult PutPropietario(Propietario propietario)
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);
				var propietarioDb = User.Identity != null
					? contexto.Usuarios.Find(userId)
					: null;

				if (propietarioDb == null) return NotFound();

				if (propietario.Id != propietarioDb.Id) return BadRequest();

				if (
					//long.is.IsNullOrEmpty(propietario.Dni) ||
					string.IsNullOrEmpty(propietario.Nombre) ||
					string.IsNullOrEmpty(propietario.Apellido) ||
					string.IsNullOrEmpty(propietario.Email) ||
					string.IsNullOrEmpty(propietario.Telefono)
				)
				{
					return BadRequest("Nungun campo puede ser vacio");
				}

				propietarioDb.Dni = propietario.Dni;
				propietarioDb.Nombre = propietario.Nombre;
				propietarioDb.Apellido = propietario.Apellido;
				propietarioDb.Email = propietario.Email;
				propietarioDb.Telefono = propietario.Telefono;

				contexto.Usuarios.Update(propietarioDb);
				contexto.SaveChanges();

				return Ok(propietario);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}