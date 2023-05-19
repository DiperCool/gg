using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Teams.Query.GetTeamates;
public class GetTeammatesQuery : IRequest<List<TeammateDTO>>
{
    public Guid TeamId { get; set; }
}

public class GetTeammatesQueryHandler : IRequestHandler<GetTeammatesQuery, List<TeammateDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetTeammatesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TeammateDTO>> Handle(GetTeammatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamUsers.Where(x =>!x.Team.IsDeleted && x.TeamId == request.TeamId)
            .Select(x => x.Teammate).ProjectToListAsync<TeammateDTO>(_mapper.ConfigurationProvider);
    }
}