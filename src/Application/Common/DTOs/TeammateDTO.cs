using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs;

public class TeammateDTO: IMapFrom<Teammate>
{
    public TeammateType TeammateType { get; set; }
    public PublicUserDTO User { get; set; } = null!;
}