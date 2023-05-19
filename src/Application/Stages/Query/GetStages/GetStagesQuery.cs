using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Events;
using MediatR;

namespace CleanArchitecture.Application.Stages.Query.GetStages;

public class GetStagesQuery : IRequest<List<StageDTO>>
{
    public string EventId { get; set; } = String.Empty;
}

public class GetStagesQueryHandler : IRequestHandler<GetStagesQuery,List<StageDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetStagesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<StageDTO>> Handle(GetStagesQuery request, CancellationToken cancellationToken)
    {
        List<StageDTO> stageDtos=await _context.Stages.Where(x => x.EventId == request.EventId).ProjectToListAsync<StageDTO>(_mapper.ConfigurationProvider);
        foreach (StageDTO  stage in stageDtos)
        {
            foreach (GroupDTO group in stage.Groups)
            {
                group.Participants =
                    group.AllParticipants.Where(x => !x.IsReserve && !x.IsPaid).OrderBy(x => x.SlotId).ToList();
                group.PaidParticipants =
                    group.AllParticipants.Where(x => !x.IsReserve && x.IsPaid).OrderBy(x => x.SlotId).ToList();
                group.ReserveParticipants =
                    group.AllParticipants.Where(x => x.IsReserve && !x.IsPaid).OrderBy(x => x.SlotId).ToList();
            }
        }
        return stageDtos;
    }
}

