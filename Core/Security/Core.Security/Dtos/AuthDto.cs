using Core.Dtos;

namespace Core.Security.Dtos
{
    public class AuthDto
    {
        public TokenModel Token { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
