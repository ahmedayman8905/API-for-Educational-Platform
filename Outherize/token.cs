using Api_1.Contract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Api_1.Outherize;

public class token(IOptions<JwtOptions> options)
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expiresIn) GenerateToken(StudentResponse user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        // Define claims for the token
        Claim[] claims = new[]
        {
           
            new Claim(JwtRegisteredClaimNames.Sub, user.id),
            new Claim(JwtRegisteredClaimNames.Gender, user.Gender!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName !),
            new Claim("Phone", user.Phone !),
            new Claim("Password", user.Password !),
            new(nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),
            new(nameof(permissions), JsonSerializer.Serialize(permissions), JsonClaimValueTypes.JsonArray),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //// Create the key for signing the token
        //var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjacB"));

        //// Define signing credentials with security algorithm
        //var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


        ////
        //var expiresIn = 30;

        //var token = new JwtSecurityToken(
        //    issuer: "SurveyBasketApp",
        //    audience: "SurveyBasketApp users",
        //    claims: claims,
        //    expires: DateTime.UtcNow.AddMinutes(expiresIn),
        //    signingCredentials: signingCredentials
        //);

        // use option pattern

        // Create the key for signing the token
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        // Define signing credentials with security algorithm
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,    
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials
        );



        //return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn * 60);

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _options.ExpiryMinutes * 60);

    }

    public string? ValisationToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            tokenHandler.ValidateToken(token, validationParameters: new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }

}
