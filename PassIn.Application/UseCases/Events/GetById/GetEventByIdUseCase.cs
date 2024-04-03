using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById;
public class GetEventByIdUseCase
{
    public ResponseEventJson Execute(Guid id)
    {
        var dbContext = new PassInDbContext();

        //Exemplo de pesquisa mais complexa no db
        //dbContext.Events.FirstOrDefault(Event => Event.Id == id && Event.Title == "master");
        var entity = dbContext
            .Events
            .Include(Event => Event.Attendees)
            .FirstOrDefault(Event => Event.Id.Equals(id));
             
        if(entity is null) throw new NotFoundException("This event doesn't exist.");

        return new ResponseEventJson 
        { 
            Id = entity.Id, 
            Title = entity.Title, 
            Details = entity.Details,
            MaximumAttendees = entity.Maximum_Attendees,
            AttendeesAmount = entity.Attendees.Count(),
        };

    }
}
