using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Groups.Query.GetGroups;

public class GetGroupsQuery: IRequest<List<GroupDTO>>
{
    public string StageId { get; set; } = String.Empty;
}

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, List<GroupDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GroupDTO>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Stages.AnyAsync(x => x.Id == request.StageId, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("Not found");
        }
        List<GroupDTO> groupDtos = await _context.Groups.Where(x => x.StageId == request.StageId)
            .ProjectToListAsync<GroupDTO>(_mapper.ConfigurationProvider);
        foreach (GroupDTO group in groupDtos)
        {
            group.Participants =
                group.AllParticipants.Where(x => !x.IsReserve && !x.IsPaid).OrderBy(x => x.SlotId).ToList();
            group.PaidParticipants =
                group.AllParticipants.Where(x => !x.IsReserve && x.IsPaid).OrderBy(x => x.SlotId).ToList();
            group.ReserveParticipants =
                group.AllParticipants.Where(x => x.IsReserve && !x.IsPaid).OrderBy(x => x.SlotId).ToList();
        }

        return groupDtos;
    }
}