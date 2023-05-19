using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Organizers.Commands.UpdateLogoOrganizer;
[EmployeeAuthorize("Organizer")]
public class UpdateLogoOrganizerCommand: IRequest<MediaFileDTO>
{
    public FileModel? File { get; set; }
}

public class UpdateLogoOrganizerCommandHandler : IRequestHandler<UpdateLogoOrganizerCommand, MediaFileDTO>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUser;

    private IFileService _fileService;

    private IMapper _mapper;

    public UpdateLogoOrganizerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IFileService fileService, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<MediaFileDTO> Handle(UpdateLogoOrganizerCommand request, CancellationToken cancellationToken)
    {
        MediaFile? mediaFileDelete =
            await _context.MediaFiles.FirstOrDefaultAsync(x => x.OrganizerProfile!.EmployeeId == _currentUser.UserIdGuid,
                cancellationToken: cancellationToken);
        if (mediaFileDelete != null)
        {
            _fileService.DeleteFile(mediaFileDelete.FullPath);
            _context.MediaFiles.Remove(mediaFileDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        if (request.File != null)
        {
            MediaFile md = _fileService.SaveFile(request.File);
            md.OrganizerProfileId = (await _context.EmployeeProfiles.FirstAsync(x => x.EmployeeId == _currentUser.UserIdGuid, cancellationToken: cancellationToken)).Id;
            await _context.MediaFiles.AddAsync(md, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MediaFile, MediaFileDTO>(md);
        }

        return null!;
    }
}