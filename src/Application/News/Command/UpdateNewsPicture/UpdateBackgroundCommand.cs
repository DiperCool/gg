using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Command.UpdateNewsPicture;
[EmployeeAuthorize("NewsEditor")]
public class UpdateBackgroundCommand : IRequest<MediaFileDTO>
{
    public Guid NewsId { get; set; }
    public FileModel? File { get; set; }
}

public class UpdateBackgroundCommandHandler : IRequestHandler<UpdateBackgroundCommand, MediaFileDTO>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    private IFileService _fileService;

    public UpdateBackgroundCommandHandler(IApplicationDbContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<MediaFileDTO> Handle(UpdateBackgroundCommand request, CancellationToken cancellationToken)
    {
        MediaFile? mediaFileDelete =
            await _context.MediaFiles.FirstOrDefaultAsync(x => x.BackgroundNews!.Id ==request.NewsId,
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
            md.BackgroundNewsId = (await _context.News.FirstAsync(x => x.Id == request.NewsId, cancellationToken: cancellationToken)).Id;
            await _context.MediaFiles.AddAsync(md, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MediaFile, MediaFileDTO>(md);
        }

        return null!;
    }
}