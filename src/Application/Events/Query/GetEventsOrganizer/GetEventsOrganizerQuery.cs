using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Events;
using MediatR;

namespace CleanArchitecture.Application.Events.Query.GetEventsOrganizer;

public class GetEventsOrganizerQuery : IRequest<List<EventWithoutStageDTO>>
{
    public Guid OrganizerId { get; set; }
}

public class GetEventsOrganizerQueryHandler : IRequestHandler<GetEventsOrganizerQuery,List<EventWithoutStageDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetEventsOrganizerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EventWithoutStageDTO>> Handle(GetEventsOrganizerQuery request, CancellationToken cancellationToken)
    {
        return await _context.Events.Where(x=>x.OrganizerId==request.OrganizerId).ProjectToListAsync<EventWithoutStageDTO>(_mapper.ConfigurationProvider);

    }
}