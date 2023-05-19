using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Command.EditNews;
[EmployeeAuthorize("NewsEditor")]
public class EditNewsCommand : IRequest<Unit>
{
    public Guid NewsId { get; set; }
    public string Title { get; set; }= String.Empty;
    public string Content { get; set; } =String.Empty;
    public NewsType Type { get; set; }
}

public class EditNewsCommandHandler : IRequestHandler<EditNewsCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;

    public EditNewsCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log)
    {
        _context = context;
        _current = current;
        _log = log;
    }

    public async Task<Unit> Handle(EditNewsCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.News news =
            await _context.News.FirstOrDefaultAsync(x => x.Id == request.NewsId && x.CreatorId == _current.UserIdGuid, cancellationToken: cancellationToken)
            ?? throw new ForbiddenAccessException();
        news.Content = request.Content;
        news.Title = request.Title;
        news.Type = request.Type;
        _context.News.Update(news);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.DeleteNews, _current.UserIdGuid, news.Id);

        return Unit.Value;
    }
}