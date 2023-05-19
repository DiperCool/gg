using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Profiles.Command.UpdateProfilePicture;
[Authorize]
public class UpdateProfilePictureCommand: IRequest<MediaFileDTO>
{
    public FileModel? File { get; set; }
}

public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, MediaFileDTO>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUser;
    private IFileService _fileService;
    private IMapper _mapper;

    public UpdateProfilePictureCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IFileService fileService, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<MediaFileDTO> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {
        MediaFile? mediaFileDelete =
            await _context.MediaFiles.FirstOrDefaultAsync(x => x.Profile!.UserId == _currentUser.UserIdGuid,
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
            md.ProfileId = (await _context.Profiles.FirstAsync(x => x.UserId == _currentUser.UserIdGuid, cancellationToken: cancellationToken)).Id;
            await _context.MediaFiles.AddAsync(md, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MediaFile, MediaFileDTO>(md);
        }

        return null!;
    }
}
