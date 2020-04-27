using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.Users.Queries.GetUser
{
    public class GetUserModel : IMapFrom<User>
    {
        public string UserStatusTypeName { get; set; }
        public GitHubUserModel GitHubUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
