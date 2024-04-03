using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventsId;
public class GetAllAttendeesByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;
    public GetAllAttendeesByEventIdUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseAllAttendeesjson Execute(Guid eventId)
    {
        //_dbContext.Attendees.Where(attendee => attendee.Event_Id == eventId).ToList();

        var entity = _dbContext
            .Events
            .Include(Event => Event.Attendees)
            .ThenInclude(Attendees => Attendees.CheckIn)
            .FirstOrDefault(Event => Event.Id.Equals(eventId));

        if (entity is null)
            throw new NotFoundException("This event doesn't exist.");

        return new ResponseAllAttendeesjson 
        {
            Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email,
                CreatedAt = attendee.Created_At,
                CheckedInAt = attendee.CheckIn?.Created_at,
            }).ToList(),
        };
    }
}
