

using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Command.UploadPicture;
[EmployeeAuthorize("NewsEditor")]
public class UploadPictureCommand : IRequest<MediaFileDTO>
{
    public Guid NewsId { get; set; }
    public FileModel Picture { get; set; } = null!;
}

public class UploadPictureCommandHandler : IRequestHandler<UploadPictureCommand, MediaFileDTO>
{
    private IFileService _fileService;
    private IMapper _mapper;
    private IApplicationDbContext _context;

    private ICurrentUserService _currentUser;

    public UploadPictureCommandHandler(IFileService fileService, IMapper mapper, IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<MediaFileDTO> Handle(UploadPictureCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.News news = await _context.News.FirstOrDefaultAsync(x => x.Id == request.NewsId && x.CreatorId==_currentUser.UserIdGuid, cancellationToken: cancellationToken) ??
                                    throw new BadRequestException("Not found");
        
        MediaFile fm = _fileService.SaveFile(request.Picture);
        fm.NewsId = news.Id;
        await _context.MediaFiles.AddAsync(fm, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MediaFile, MediaFileDTO>(fm);
    }
}