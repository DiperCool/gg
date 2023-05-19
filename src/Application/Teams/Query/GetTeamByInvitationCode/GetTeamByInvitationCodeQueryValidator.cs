using FluentValidation;

namespace CleanArchitecture.Application.Teams.Query.GetTeamByInvitationCode;

public class GetTeamByInvitationCodeQueryValidator : AbstractValidator<GetTeamByInvitationCodeQuery>
{
    public GetTeamByInvitationCodeQueryValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}