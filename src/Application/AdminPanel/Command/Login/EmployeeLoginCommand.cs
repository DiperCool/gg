using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.Login;

public class EmployeeLoginCommand: IRequest<TokenResult>
{
    public string Nickname { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

public class EmployeeLoginCommandHandler : IRequestHandler<EmployeeLoginCommand, TokenResult>
{
    private IApplicationDbContext _context;
    private IJWTService _jwtService;
    private IHashPassword _hashPassword;
    private IEmployeeLogService _log;
    public EmployeeLoginCommandHandler(IApplicationDbContext context, IJWTService jwtService, IHashPassword hashPassword, IEmployeeLogService log)
    {
        _context = context;
        _jwtService = jwtService;
        _hashPassword = hashPassword;
        _log = log;
    }

    public async Task<TokenResult> Handle(EmployeeLoginCommand request, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.Include(x=>x.Role).FirstOrDefaultAsync(x=>x.Nickname==request.Nickname&&x.Password== _hashPassword.Hash(request.Password)&& !x.IsDeleted, cancellationToken: cancellationToken)?? throw new BadRequestException("Email or password is incorrect");
        employee.Loggings.Add(new UserLogging()
        {
            Time = DateTime.UtcNow
        });
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(employee.Id, LogsEnum.Logging, employee.Id);
        return await _jwtService.GenerateToken(employee.Id, DefaultClaims.GetEmployeeClaims(employee),user:false);
    }
}