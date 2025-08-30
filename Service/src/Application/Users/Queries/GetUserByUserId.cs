namespace Application.Users.Queries;

public record GetUserByIdQuery(int Id);

public record GetUserByUserIdQuery(string UserId);