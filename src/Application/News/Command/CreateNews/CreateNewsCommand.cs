using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Command.CreateNews;
[EmployeeAuthorize("NewsEditor")]
public class CreateNewsCommand : IRequest<Guid>
{
    public string Title { get; set; }= String.Empty;
    public string Content { get; set; } =String.Empty;
    public NewsType Type { get; set; }
    public FileModel? Background { get; set; }
    public List<string> Pictures { get; set; } = new();

}

public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, Guid>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;
    private IFileService _file;

    public CreateNewsCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log, IFileService file)
    {
        _context = context;
        _current = current;
        _log = log;
        _file = file;
    }

    public async Task<Guid> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.News news = new()
        {
            Type = request.Type, Content = request.Content, Title = request.Title,CreatorId = _current.UserIdGuid, CreatedAt = DateTime.UtcNow, IsDeleted = false
        };
        if (request.Background != null)
        {
            MediaFile md = _file.SaveFile(request.Background);
            news.Background = md;
            md.BackgroundNews = news;
        }
        request.Pictures = request.Pictures.Select(x => "api/" + x).ToList();
        List<MediaFile> pictures =
            await _context.MediaFiles.Where(x => request.Pictures.Contains(x.ShortPath)).ToListAsync(cancellationToken: cancellationToken);
        foreach (MediaFile picture in pictures)
        {
            news.Pictures.Add(picture);
            picture.News = news;
        }
        NewsEditorProfile newsEditorProfile =
            await _context.NewsEditorProfiles.FirstAsync(x => x.Employee.Id == _current.UserIdGuid, cancellationToken: cancellationToken);
        newsEditorProfile.CreatedNews.Add(news);
        await _context.News.AddAsync(news, cancellationToken);
        _context.MediaFiles.UpdateRange(pictures);
        _context.NewsEditorProfiles.Update(newsEditorProfile);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.CreateNews, _current.UserIdGuid, news.Id);

        return news.Id;
    }
}