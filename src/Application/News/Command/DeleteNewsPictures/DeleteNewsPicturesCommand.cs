using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Command.DeleteNewsPictures;
[EmployeeAuthorize("NewsEditor")]
public class DeleteNewsPicturesCommand : IRequest<Unit>
{
    public Guid NewsId { get; set; }
    public List<string> Pictures { get; set; } = new();
}

public class DeleteNewsPicturesCommandHandler : IRequestHandler<DeleteNewsPicturesCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUser;
    private IFileService _file;

    public DeleteNewsPicturesCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IFileService file)
    {
        _context = context;
        _currentUser = currentUser;
        _file = file;
    }

    public async Task<Unit> Handle(DeleteNewsPicturesCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.News.AnyAsync(x => x.CreatorId == _currentUser.UserIdGuid, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("You are not a creator");
        }

        request.Pictures = request.Pictures.Select(x => "api/" + x).ToList();
        List<MediaFile> remove =
            await _context.MediaFiles.Where(x => request.Pictures.Contains(x.ShortPath) && x.NewsId==request.NewsId).ToListAsync(cancellationToken: cancellationToken);
        foreach (MediaFile file in remove)
        {
            _file.DeleteFile(file.FullPath);
        }
        _context.MediaFiles.RemoveRange(remove);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}