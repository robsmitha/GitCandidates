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

            var jobs = new List<Job>
            {
                new Job
                {
                    Name = "Full Stack Engineer",
                    Description = "We are seeking a Full Stack Engineer for Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                    Location = "Beverly Hills, CA.",
                    Latitide = 34.077200,
                    Longitude = -118.422450,
                    Company = "Big Software Company",

                },
                new Job
                {
                    Name = "Front End Engineer",
                    Description = "We are looking for a Front End Engineer for Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                    Location = "Tampa, FL.",
                    Latitide = 27.950575,
                    Longitude = -82.457176,
                    Company = "Startup Software Company",
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new Job
                {
                    Name = "Senior Software Engineer",
                    Description = "Are you a Senior Software for Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                    Location = "Redmond, WA.",
                    Latitide = 47.751076,
                    Longitude = -120.740135,
                    Company = "Saas Company",
                    CreatedAt = DateTime.Now.AddDays(-4)
                },
                new Job
                {
                    Name = "Cloud Engineer",
                    Description = "Come join our team of cloud engineering developers Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                    Location = "New York, NY.",
                    Latitide = 40.712776,
                    Longitude = -74.005974,
                    Company = "Financial Software Company",
                    CreatedAt = DateTime.Now.AddDays(-8)
                }
            };
            context.Jobs.AddRange(jobs);
            context.SaveChanges();
        }
    }
}
