namespace PCParts.Storage.Common.Authentication.JwtProvider;

public interface IJwtTokenProvider
{
    string Create(Guid id, string email);
}