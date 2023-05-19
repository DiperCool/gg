using CleanArchitecture.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace CleanArchitecture.Application.Common.DTOs;

public class MediaFileDTO: IMapFrom<MediaFile>
{ 
    public string Path { get; set; } = String.Empty;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<MediaFile, MediaFileDTO>()
            .ForMember(x=>x.Path, opt=>opt.MapFrom(x=>x.ShortPath.Replace("api/","")));
    }
}