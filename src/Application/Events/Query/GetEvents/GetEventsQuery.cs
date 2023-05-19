using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.Events;
using MediatR;

namespace CleanArchitecture.Application.Events.Query.GetEvents;

public class GetEventsQuery : IRequest<List<EventWithoutStageDTO>>
{
    
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventWithoutStageDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetEventsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EventWithoutStageDTO>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Events.ProjectToListAsync<EventWithoutStageDTO>(_mapper.ConfigurationProvider);

    }
}