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
            var confirmed = new UserStatusType
            {
                Name = "Confirmed",
                Description = "The user has confirmed their email is active and is ready for opportunities."
            };
            var userStatusTypes = new List<UserStatusType>
            {
                new UserStatusType
                {
                    Name = "Submitted",
                    Description = "The user has signed up and needs to confirm their email is active and receiving messages from the system."
                },
                confirmed,
                new UserStatusType
                {
                    Name = "Banned",
                    Description = "The user has been banned from any opportunities."
                }
            };
            context.UserStatusTypes.AddRange(userStatusTypes);
            context.SaveChanges();

            var robsmitha = new User
            {
                GitHubLogin = "robsmitha",
                UserStatusTypeID = confirmed.ID
            };
            context.Users.Add(robsmitha);
            context.SaveChanges();

            var gitCandidates = new Company
            {
                GitHubLogin = "GitCandidates"
            };
            context.Companies.Add(gitCandidates);
            context.SaveChanges();

            var fullstack = new Job
            {
                Name = "Full Stack Engineer",
                Description = "We are seeking a Full Stack Engineer for Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                CompanyID = gitCandidates.ID,
                UserID = robsmitha.ID,
                PostAt = DateTime.Now.AddMinutes(5)
            };
            var frontend = new Job
            {
                Name = "Front End Engineer",
                Description = "We are looking for a Front End Engineer for Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                CompanyID = gitCandidates.ID,
                PostAt = DateTime.Now.AddDays(-2),
                UserID = robsmitha.ID
            };
            var senior = new Job
            {
                Name = "Senior Software Engineer",
                Description = "Are you a Senior Software for Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                CompanyID = gitCandidates.ID,
                PostAt = DateTime.Now.AddDays(-4),
                UserID = robsmitha.ID
            };
            var cloud = new Job
            {
                Name = "Cloud Engineer",
                Description = "Come join our team of cloud engineering developers Lorem ipsum dolor sit amet, consectetur adipisicing elit. A deserunt neque tempore recusandae animi soluta quasi? Asperiores rem dolore eaque vel, porro, soluta unde debitis aliquam laboriosam. Repellat explicabo, maiores! Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis optio neque consectetur consequatur magni in nisi, natus beatae quidem quam odit commodi ducimus totam eum, alias, adipisci nesciunt voluptate. Voluptatum.",
                CompanyID = gitCandidates.ID,
                PostAt = DateTime.Now.AddDays(-8),
                UserID = robsmitha.ID
            };

            var jobs = new List<Job>
            {
                fullstack, frontend, senior, cloud
            };
            context.Jobs.AddRange(jobs);
            context.SaveChanges();

            var jobLocations = new List<JobLocation>
            {
                new JobLocation
                {
                    City = "Beverly Hills",
                    StateAbbreviation = "CA",
                    Latitude = 34.077200,
                    Longitude = -118.422450,
                    JobID = fullstack.ID
                },
                new JobLocation
                {
                    City = "Tampa",
                    StateAbbreviation = "FL",
                    Latitude = 27.950575,
                    Longitude = -82.457176,
                    JobID = frontend.ID
                },
                new JobLocation
                {
                    City = "Redmond",
                    StateAbbreviation = "WA",
                    Latitude = 47.751076,
                    Longitude =  -120.740135,
                    JobID = senior.ID
                },
                new JobLocation
                {
                    City = "New York",
                    StateAbbreviation = "NY",
                    Latitude = 40.712776,
                    Longitude =  -74.005974,
                    JobID = cloud.ID
                },
            };
            context.JobLocations.AddRange(jobLocations);

            context.SaveChanges();


        }
    }
}
