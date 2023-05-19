using AutoMapper;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Command.UpdateTeam;
[Authorize]
public class UpdateTeamCommand : IRequest<Unit>
{
    public Guid TeamId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Tag { get; set; } = String.Empty;
}

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public UpdateTeamCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        Team? team = await _context.Teams.FirstOrDefaultAsync(
            x => !x.IsDeleted && x.Id == request.TeamId && x.CreatorId == _current.UserIdGuid, cancellationToken: cancellationToken);
        if(team==null)
        {
            throw new BadRequestException("You are not creator of this command");
        }

        team.Tag = request.Tag;
        team.Title = request.Title;
        _context.Teams.Update(team);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}