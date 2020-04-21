using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IApplicationContext context)
        {
            if (!context.EnsureCreated())
            {
                return; //db was already created or error occurred
            }

            var userStatusTypes = new List<UserStatusType>
            {
                new UserStatusType
                {
                    Name = "Submitted / Unconfirmed",
                    Description = "The user has signed up and needs to confirm their email."
                },
                new UserStatusType
                {
                    Name = "Confirmed",
                    Description = "The user has confirmed their email and is ready for consideration."
                },
                new UserStatusType
                {
                    Name = "Closed",
                    Description = "The user has been closed."
                }
            };
            context.UserStatusTypes.AddRange(userStatusTypes);
            context.SaveChanges();
        }
    }
}
