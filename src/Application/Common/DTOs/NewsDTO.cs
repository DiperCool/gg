using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Common.DTOs;

public class NewsDTO : IMapFrom<Domain.Entities.News>
{
    public Guid Id { get; set; }
    public NewsType Type { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
    
    public PublicEmployeeDTO Creator { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public MediaFileDTO? Background { get; set; }
    public List<MediaFileDTO> Pictures { get; set; } = new();
    public bool IsNew { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.News, NewsDTO>()
            .ForMember(x=>x.IsNew, opt=>opt.MapFrom(x=>x.CreatedAt.AddDays(7)>=DateTime.UtcNow));
    }
}