﻿using AutoMapper;

namespace CleanArchitecture.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(AutoMapper.Profile profile) => profile.CreateMap(typeof(T), GetType());
}
