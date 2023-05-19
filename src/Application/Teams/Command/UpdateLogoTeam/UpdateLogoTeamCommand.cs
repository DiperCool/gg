using System.Runtime.Intrinsics.X86;
using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Command.UpdateLogoTeam;

public class UpdateLogoTeamCommand : IRequest<MediaFileDTO>
{
    public Guid TeamId { get; set; }
    public FileModel? File { get; set; }
}

public class UpdateLogoTeamCommandHandler : IRequestHandler<UpdateLogoTeamCommand, MediaFileDTO>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IMapper _mapper;

    private IFileService _fileService;

    public UpdateLogoTeamCommandHandler(IApplicationDbContext context, ICurrentUserService current, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _current = current;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<MediaFileDTO> Handle(UpdateLogoTeamCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Teams.AnyAsync(x => !x.IsDeleted && x.CreatorId == _current.UserIdGuid && x.Id==request.TeamId, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("You are not creator of this command");
        }
        MediaFile? mediaFileDelete = await _context.MediaFiles.FirstOrDefaultAsync(
            x =>x.Team!.CreatorId == _current.UserIdGuid && x.TeamId==request.TeamId, cancellationToken: cancellationToken);
        if (mediaFileDelete != null)
        {
            _fileService.DeleteFile(mediaFileDelete.FullPath);
            _context.MediaFiles.Remove(mediaFileDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        if (request.File != null)
        {
            MediaFile md = _fileService.SaveFile(request.File);
            md.TeamId = request.TeamId;
            await _context.MediaFiles.AddAsync(md, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MediaFile, MediaFileDTO>(md);
        }

        return null!;
    }
}