using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IGenerateOAuthAccessToken
    {
        string code { get; set; }
    }
}
