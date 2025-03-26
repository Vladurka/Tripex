namespace Auth.API.DTOs;

public record RegisterResponse(
    TokenModel Token,
    Guid Id
);
