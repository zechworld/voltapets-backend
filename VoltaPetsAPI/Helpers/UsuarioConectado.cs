using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VoltaPetsAPI.Helpers
{
    public class UsuarioConectado
    {
        public static int ObtenerCodigo(ClaimsIdentity claims)
        {
            var codUser = claims.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            int codigoUsuario;

            if (int.TryParse(codUser, out int id))
            {
                codigoUsuario = id;
            }
            else
            {
                codigoUsuario = 0;
            }

            return codigoUsuario;
        }
    }
}
