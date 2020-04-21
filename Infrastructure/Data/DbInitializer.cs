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
                    Name = "Submitted",
                    Description = "The user has signed up and needs to confirm their email is active and receiving messages from the system."
                },
                new UserStatusType
                {
                    Name = "Confirmed",
                    Description = "The user has confirmed their email is active and is ready for opportunities."
                },
                new UserStatusType
                {
                    Name = "Banned",
                    Description = "The user has been banned from any opportunities."
                }
            };
            context.UserStatusTypes.AddRange(userStatusTypes);
            context.SaveChanges();
        }
    }
}
