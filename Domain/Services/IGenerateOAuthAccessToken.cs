using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public interface IGenerateOAuthAccessToken
    {
        string code { get; set; }
    }
}
