using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Jobs.Commands.CreateJobApplication
{
    public class CreateJobApplicationCommand : IRequest<bool>
    {
        public int UserID { get; set; }
        public int JobID { get; set; }
        public List<JobApplicationQuestionResponseModel>  Responses { get; set; }
        public CreateJobApplicationCommand(CreateJobApplicationModel model, int userId)
        {
            UserID = userId;
            JobID = model.JobID;
            Responses = model.Responses;
        }
    }
    public class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand, bool>
    {
        private readonly IApplicationContext _context;

        public CreateJobApplicationCommandHandler(IApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var underReview = _context.JobApplicationStatusTypes.FirstOrDefault();
            if (underReview == null) return false;

            var jobApplication = new JobApplication
            {
                JobID = request.JobID,
                UserID = request.UserID,
                JobApplicationStatusTypeID = underReview.ID
            };
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync(cancellationToken);

            request.Responses.ForEach(r => _context.JobApplicationQuestionResponses.Add(new JobApplicationQuestionResponse
            {
                JobApplicationQuestionID = r.JobApplicationQuestionID,
                Response = r.Response.ToString()
            }));

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
