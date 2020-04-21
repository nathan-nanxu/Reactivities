using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
  public class Create
  {
    public class Command : IRequest<CommentDTO>
    {
      public string Body { get; set; }
      public Guid ActivityId { get; set; }
      public string Username { get; set; }
    }

    public class Handler : IRequestHandler<Command, CommentDTO>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      public Handler(DataContext context, IMapper mapper)
      {
        _mapper = mapper;
        _context = context;
      }

      public async Task<CommentDTO> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities.FindAsync(request.ActivityId);

        if (activity == null)
          throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });

        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

        var comment = new Comment
        {
          Author = user,
          Activity = activity,
          CreatedAt = DateTime.Now,
          Body = request.Body
        };

        activity.Comments.Add(comment);

        var success = await _context.SaveChangesAsync() > 0;

        if (success) return _mapper.Map<CommentDTO>(comment);

        throw new Exception("Problem saving changes.");
      }
    }
  }
}