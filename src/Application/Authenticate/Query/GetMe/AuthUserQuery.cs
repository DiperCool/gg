using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Query.GetMe
{
    [Authorize]
    public class AuthUserQuery : IRequest<UserDTO>
    {

    }

    public class GetMeQueryHandler : IRequestHandler<AuthUserQuery, UserDTO>
    {
        ICurrentUserService _currentUserService;

        private IApplicationDbContext _context;
        private IMapper _mapper;

        public GetMeQueryHandler(ICurrentUserService currentUserService, IApplicationDbContext context, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(AuthUserQuery request, CancellationToken cancellationToken)
        {
            UserDTO uset =  await _context.Users.Where(x => x.Id == _currentUserService.UserIdGuid)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider).FirstAsync(cancellationToken);

            return uset;
        }
    }
}