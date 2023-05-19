using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure.Services;

public class GeneratorId : IGeneratorId
{
    public int Generate()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        return Math.Abs(bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24));
    }
}