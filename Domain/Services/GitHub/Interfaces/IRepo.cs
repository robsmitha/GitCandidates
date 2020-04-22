using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.GitHub.Interfaces
{
    public interface IRepo
    {
        int id { get; set; }
        string url { get; set; }
        string name { get; set; }
    }
}
