using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }= String.Empty;
        public string Issuer { get; set; }= String.Empty;
        public string Audience { get; set; }= String.Empty;
        public int ExpirationMinutes { get; set; }
    }
}