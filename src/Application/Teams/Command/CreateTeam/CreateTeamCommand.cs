using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Command.CreateTeam;
[Authorize]
public class CreateTeamCommand: IRequest<TeamDTO>
{
    public string Title { get; set; } = String.Empty;
    public string Tag { get; set; } = String.Empty;
    public FileModel? Picture { get; set; } = null!;
}

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, TeamDTO>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IMapper _mapper;

    private IFileService _file;

    public CreateTeamCommandHandler(IApplicationDbContext context, ICurrentUserService current, IMapper mapper, IFileService file)
    {
        _context = context;
        _current = current;
        _mapper = mapper;
        _file = file;
    }

    public async Task<TeamDTO> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Teams.AnyAsync(x => x.Tag == request.Tag, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("Tag exists");
        }
        Team team = new()
        {
            CreatedAt = DateTime.UtcNow, ManagerInvitationCodeLink = Guid.NewGuid().ToString(),CreatorId = _current.UserIdGuid, Tag = request.Tag, Title = request.Title, InvitationCodeLink = Guid.NewGuid().ToString(), NumberOfMembers = 1
        };
        if (request.Picture != null)
        {
            MediaFile md = _file.SaveFile(request.Picture);
            team.Logo = md;
        }
        await _context.Teams.AddAsync(team, cancellationToken);
        TeamUser tu = new()
        { 
            Teammate = new Teammate()
            {
                TeammateType = TeammateType.Leader,
                UserId = _current.UserIdGuid
            }, 
            Team = team,
            UserId = _current.UserIdGuid,
        };
        await _context.TeamUsers.AddAsync(tu, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<Team, TeamDTO>(team);
    }
}