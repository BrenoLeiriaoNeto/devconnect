using DevConnect.Application.Contracts.Models.ViewModels;
using MediatR;

namespace DevConnect.Application.Services.Queries;

public class GetUserByIdQuery(Guid userId) : IRequest<UserProfileViewModel>
{
    public Guid UserId { get; } = userId;
}