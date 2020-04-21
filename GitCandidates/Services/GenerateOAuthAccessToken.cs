using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitCandidates.Services
{
    public class GenerateOAuthAccessToken : IGenerateOAuthAccessToken
    {
        public string code { get; set; }
        public string state { get; set; }
    }
}
