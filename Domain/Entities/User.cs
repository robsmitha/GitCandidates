﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string GitHubLogin { get; set; }
        public string GitHubToken { get; set; }
        public string JWT { get; set; }
        public int UserStatusTypeID { get; set; }
        [ForeignKey("UserStatusTypeID")]
        public UserStatusType UserStatusType { get; set; }
    }
}
