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
                Description = "The user has confirmed their email is active and is ready for opportunities.",
                CanApply = true
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

            //add user
            var robsmitha = new User
            {
                GitHubLogin = "robsmitha",
                UserStatusTypeID = confirmed.ID
            };
            context.Users.Add(robsmitha);
            context.SaveChanges();

            //add company
            var gitCandidates = new Company
            {
                GitHubLogin = "GitCandidates"
            };
            context.Companies.Add(gitCandidates);
            context.SaveChanges();

            #region Response types
            var textResponse = new ResponseType
            {
                Name = "TextResponse",
                Description = "A write in response question that renders a text input field.",
                Input = "text"
            };
            var numberResponse = new ResponseType
            {
                Name = "NumberResponse",
                Description = "A write in response question that renders a number input field.",
                Input = "number"
            };
            var yesNo = new ResponseType
            {
                Name = "YesNo",
                Description = "A yes/no question that renders two yes/mp radio buttons.",
                Input = "radio"
            };
            context.ResponseTypes.Add(yesNo);
            context.ResponseTypes.Add(numberResponse);
            context.ResponseTypes.Add(textResponse);
            context.SaveChanges();
            #endregion

            #region Validation rules
            var isRequired = new ValidationRule
            {
                Name = "Is Required",
                Description = "The field in requierd",
                Key = "isRequired"
            };
            var minLength = new ValidationRule
            {
                Name = "Minimum Length",
                Description = "The field has a minimum length",
                Key = "minLength"
            };
            var maxLength = new ValidationRule
            {
                Name = "Maximum Length",
                Description = "The field has a maximum length",
                Key = "maxLength"
            };
            context.ValidationRules.Add(isRequired);
            context.ValidationRules.Add(minLength);
            context.ValidationRules.Add(maxLength);
            context.SaveChanges();
            #endregion

            #region Yes/No test question
            var yesNoQ = new Question
            {
                Label = "Are you authorized to work in the U.S.?",
                CompanyID = gitCandidates.ID,
                ResponseTypeID = yesNo.ID
            };
            context.Questions.Add(yesNoQ);
            context.SaveChanges();
            var yesNoQYes = new QuestionResponse
            {
                Answer = "Yes",
                QuestionID = yesNoQ.ID,
                DisplayOrder = 1
            };
            var yesNoQNo = new QuestionResponse
            {
                Answer = "No",
                QuestionID = yesNoQ.ID,
                DisplayOrder = 2
            };
            context.QuestionResponses.Add(yesNoQYes);
            context.QuestionResponses.Add(yesNoQNo);
            context.SaveChanges();

            var yesNoQValidationIsRequired = new QuestionValidation
            {
                QuestionID = yesNoQ.ID,
                ValidationRuleID = isRequired.ID,
            };
            context.QuestionValidations.Add(yesNoQValidationIsRequired);
            context.SaveChanges();
            #endregion

            #region Write in test question
            var numberResponseQ = new Question
            {
                Label = "How many years have you professionaly developed software?",
                CompanyID = gitCandidates.ID,
                ResponseTypeID = numberResponse.ID,
                Placeholder = "Years of experience",
                Minimum = 0,
                Maximum = 99
            };
            context.Questions.Add(numberResponseQ);
            context.SaveChanges();

            var numberResponseQValidationIsRequired = new QuestionValidation
            {
                QuestionID = numberResponseQ.ID,
                ValidationRuleID = isRequired.ID,
                ValidationRuleValue = "true"
            };
            var numberResponseQValidationMaxLength = new QuestionValidation
            {
                QuestionID = numberResponseQ.ID,
                ValidationRuleID = maxLength.ID,
                ValidationRuleValue = "2"
            };
            context.QuestionValidations.Add(numberResponseQValidationIsRequired);
            context.QuestionValidations.Add(numberResponseQValidationMaxLength);
            context.SaveChanges();

            #endregion

            #region Add jobs
            var jobApplicationStatusTypes = new List<JobApplicationStatusType>
            {
                new JobApplicationStatusType
                {
                    Name = "Submitted",
                    Description = "The application has been submitted.",
                    IsActiveApplication = true
                },
                new JobApplicationStatusType
                {
                    Name = "Under Review",
                    Description = "The application is under review.",
                    IsActiveApplication = true
                },
                new JobApplicationStatusType
                {
                    Name = "Under Consideration",
                    Description = "The application is under review by the company.",
                    IsActiveApplication = true
                },
                new JobApplicationStatusType
                {
                    Name = "Scheduling Interview",
                    Description = "The an interview is being schedules.",
                    IsActiveApplication = true
                },
                new JobApplicationStatusType
                {
                    Name = "No Longer Under Consideration",
                    Description = "The application is no longer being reviewed."
                },
                new JobApplicationStatusType
                {
                    Name = "Withdrawn",
                    Description = "The application has been withdrawn by the user."
                }
            };
            context.JobApplicationStatusTypes.AddRange(jobApplicationStatusTypes);
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
            foreach (var job in jobs)
            {
                context.JobApplicationQuestions.AddRange(new[] {
                    new JobApplicationQuestion
                    {
                        JobID = job.ID,
                        QuestionID = yesNoQ.ID,
                        DisplayOrder = 1,
                    },
                    new JobApplicationQuestion
                    {
                        JobID = job.ID,
                        QuestionID = numberResponseQ.ID,
                        DisplayOrder = 2
                    }
                });
            }

            var cSharp = new Skill
            {
                Name = "C#",
                Description = "C# progamming language"
            };
            var javaScript = new Skill
            {
                Name = "JavaScript",
                Description = "JavaScript progamming language"
            };
            var sfrontend = new Skill
            {
                Name = "Frontend",
                Description = "Frontend development"
            };
            var sbackend = new Skill
            {
                Name = "Backend",
                Description = "Backend development"
            };
            var html = new Skill
            {
                Name = "HTML",
                Description = "HTML"
            };
            var css = new Skill
            {
                Name = "CSS",
                Description = "CSS"
            };
            context.Skills.AddRange(new List<Skill>
            {
                cSharp,
                javaScript,
                sfrontend,
                sbackend,
                html,
                css,
            });
            context.SaveChanges();

            context.JobSkills.AddRange(new []
            {
                //frontend
                new JobSkill
                {
                    JobID = frontend.ID,
                    SkillID = html.ID
                },
                new JobSkill
                {
                    JobID = frontend.ID,
                    SkillID = css.ID
                },
                new JobSkill
                {
                    JobID = frontend.ID,
                    SkillID = sfrontend.ID
                },
                //sr developer
                new JobSkill
                {
                    JobID = senior.ID,
                    SkillID = sbackend.ID
                },
                new JobSkill
                {
                    JobID = senior.ID,
                    SkillID = cSharp.ID
                },
                new JobSkill
                {
                    JobID = senior.ID,
                    SkillID = javaScript.ID
                },
                //cloud
                new JobSkill
                {
                    JobID = cloud.ID,
                    SkillID = sbackend.ID
                },
                new JobSkill
                {
                    JobID = cloud.ID,
                    SkillID = cSharp.ID
                },
                //full stack
                new JobSkill
                {
                    JobID = fullstack.ID,
                    SkillID = html.ID
                },
                new JobSkill
                {
                    JobID = fullstack.ID,
                    SkillID = css.ID
                },
                new JobSkill
                {
                    JobID = fullstack.ID,
                    SkillID = sfrontend.ID
                },
                new JobSkill
                {
                    JobID = fullstack.ID,
                    SkillID = sbackend.ID
                },
                new JobSkill
                {
                    JobID = fullstack.ID,
                    SkillID = cSharp.ID
                },
                new JobSkill
                {
                    JobID = fullstack.ID,
                    SkillID = javaScript.ID
                },
            });
            context.UserSkills.AddRange(new[] {
                new UserSkill
                {
                    UserID = robsmitha.ID,
                    SkillID = cSharp.ID,
                },
                new UserSkill
                {
                    UserID = robsmitha.ID,
                    SkillID = javaScript.ID,
                },
                new UserSkill
                {
                    UserID = robsmitha.ID,
                    SkillID = sfrontend.ID,
                },
                new UserSkill
                {
                    UserID = robsmitha.ID,
                    SkillID = sbackend.ID,
                },
            });
            context.SaveChanges();
            #endregion

        }
    }
}
