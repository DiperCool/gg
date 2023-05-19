using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Query.GetEventFilter;

public class GetEventFilterQuery: IRequest<List<EventWithoutStageDTO>>
{
    public List<EventType> EventTypes { get; set; } = new();
    public bool GetPaid { get; set; }
    public bool GetSponsored { get; set; }
    public bool GetFree { get; set; }

    public List<Map> Maps { get; set; } = new();
    public List<Regime> Regimes { get; set; } = new();
    public List<View> Views { get; set; } = new();
}

public class GetEventFilterQueryHandler : IRequestHandler<GetEventFilterQuery, List<EventWithoutStageDTO>>
{
    private IApplicationDbContext _dbContext;
    private IMapper _mapper;

    public GetEventFilterQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<EventWithoutStageDTO>> Handle(GetEventFilterQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Event> query = _dbContext.Events.AsNoTracking();
        if (request.EventTypes.Count != 0)
        {
            query = query.Where(x => request.EventTypes.Contains(x.EventType));
        }

        if (request is not { GetFree: true, GetPaid: true })
        {
            if (request.GetFree)
            {
                query=query.Where(x => x.IsPaid == false);
            }

            if (request.GetPaid)
            {
                query = query.Where(x => x.IsPaid);
            }
        }

        if (request.GetSponsored)
        {
            query = query.Where(x => x.IsSponsored);
        }

        if (request.Maps.Count != 0)
        {
            //query = query.Where(x => request.Maps.Contains(x.));
        }

        if (request.Regimes.Count!=0)
        {
            query = query.Where(x => request.Regimes.Contains(x.Regime));
        }
        if (request.Views.Count!=0)
        {
            query = query.Where(x => request.Views.Contains(x.View));
        }

        return await query.ProjectToListAsync<EventWithoutStageDTO>(_mapper.ConfigurationProvider);
    }
}