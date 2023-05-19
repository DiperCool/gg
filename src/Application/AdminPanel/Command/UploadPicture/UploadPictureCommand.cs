using AutoMapper;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.AdminPanel.Command.UploadPicture;

public class UploadPictureCommand : IRequest<MediaFileDTO>
{
    public FileModel Picture { get; set; } = null!;
}

public class UploadPictureCommandHandler : IRequestHandler<UploadPictureCommand, MediaFileDTO>
{
    private IFileService _fileService;

    private IMapper _mapper;

    private IApplicationDbContext _context;

    public UploadPictureCommandHandler(IFileService fileService, IMapper mapper, IApplicationDbContext context)
    {
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<MediaFileDTO> Handle(UploadPictureCommand request, CancellationToken cancellationToken)
    {
        MediaFile md = _fileService.SaveFile(request.Picture);
        await _context.MediaFiles.AddAsync(md, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MediaFile, MediaFileDTO>(md);
    }
}