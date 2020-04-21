using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public string Token { get; set; }
    }
}
