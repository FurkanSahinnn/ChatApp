namespace ChatApp.API.Core.Application.Dtos
{
    public sealed record SendMessageDto(
        int UserId,
        int ToUserId,
        string Message
        );
}
