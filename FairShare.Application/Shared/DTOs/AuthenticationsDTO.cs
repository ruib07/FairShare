namespace FairShare.Application.Shared.DTOs;

public static class AuthenticationsDTO
{
    public static class SignIn
    {
        public record Request(string Email, string Password);
        public record RefreshTokenRequest(string RefreshToken);
        public record Response(string AccessToken, string RefreshToken, string TokenType, DateTime ExpiresAt);
    }

    public static class SignUp
    {
        public record Request(string Name, string Email, string Password, byte ProfilePicture);
    }
}
