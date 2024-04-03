using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIns.CheckIn.DoCheckIn;
public class DoAttendeeCheckInUseCase
{
    private readonly PassInDbContext _dbContext;
    public DoAttendeeCheckInUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseRegisteredJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var entity = new CheckInUser
        {
            Attendee_Id = attendeeId,
            Created_at = DateTime.UtcNow,
        };

        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson 
        {
            Id = entity.Id,
        };
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee = _dbContext.Attendees.Any(attendee => attendee.Id.Equals(attendeeId));
        if (existAttendee == false)
            throw new NotFoundException("The attendee with this Id was no found");

        var existCheckIn = _dbContext.CheckIns.Any(checkIn => checkIn.Attendee_Id.Equals(attendeeId));
        if(existCheckIn)
            throw new ConflictException("Attendee can not checkin twice in the same event");
    }
}
