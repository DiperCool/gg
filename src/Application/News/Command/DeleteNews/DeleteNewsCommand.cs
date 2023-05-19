using CleanArchitecture.Application.News.Command.EditNews;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Command.DeleteNews;
[EmployeeAuthorize("NewsEditor")]
public class DeleteNewsCommand: IRequest<Unit>
{
    public Guid NewsId { get; set; }
}

public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;

    public DeleteNewsCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log)
    {
        _context = context;
        _current = current;
        _log = log;
    }

    public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.News news =
            await _context.News.FirstOrDefaultAsync(x => x.Id == request.NewsId && x.CreatorId == _current.UserIdGuid, cancellationToken: cancellationToken)
            ?? throw new ForbiddenAccessException();
        news.IsDeleted = true;
        _context.News.Update(news);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.DeleteNews, _current.UserIdGuid, news.Id);

        return Unit.Value;
    }
}