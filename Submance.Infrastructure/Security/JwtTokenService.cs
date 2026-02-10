using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Domain.Entities;                  // ✅ Para que reconozca 'Usuario'
using Submance.Application.Interfaces.Services.Auth; // ✅ Para que reconozca 'ITokenService'
using System.Security.Claims;                    // Necesario para los tokens
using Microsoft.IdentityModel.Tokens;            // Necesario para la criptografía
using System.IdentityModel.Tokens.Jwt;


namespace Submance.Application.Interfaces.Services.Auth
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
