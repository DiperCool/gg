using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Command.UpdateEventPicture;
[EmployeeAuthorize("Organizer")]
public class UpdateEventPictureCommand: IRequest<MediaFileDTO>
{
    public string EventId { get; set; } = String.Empty;
    public FileModel? File { get; set; }
}

public class UpdateEventPictureCommandHandler : IRequestHandler<UpdateEventPictureCommand, MediaFileDTO>
{
    private IApplicationDbContext _context;

    private ICurrentUserService _current;

    private IFileService _fileService;

    private IMapper _mapper;

    public UpdateEventPictureCommandHandler(IApplicationDbContext context, ICurrentUserService current, IFileService fileService, IMapper mapper)
    {
        _context = context;
        _current = current;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<MediaFileDTO> Handle(UpdateEventPictureCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Events.AnyAsync(x => x.OrganizerId == _current.UserIdGuid && x.Id==request.EventId, cancellationToken: cancellationToken))
        {
            throw new ForbiddenAccessException();
        }
        MediaFile? mediaFileDelete =
            await _context.MediaFiles.FirstOrDefaultAsync(x => x.EventId == request.EventId ,
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
            md.EventId = (await _context.Events.FirstAsync(x => x.Id == request.EventId, cancellationToken: cancellationToken)).Id;
            await _context.MediaFiles.AddAsync(md, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MediaFile, MediaFileDTO>(md);
        }

        return null!;
    }
}

