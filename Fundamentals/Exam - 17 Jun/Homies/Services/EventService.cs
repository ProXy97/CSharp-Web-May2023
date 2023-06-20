using Homies.Data;
using Homies.Data.Models;
using Homies.Interfaces;
using Homies.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Homies.Services
{
    public class EventService : IEventService
    {
        private readonly HomiesDbContext dbContext;

        public EventService(HomiesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddEventAsync(AddEventViewModel model, string userId)
        {
            try
            {
                Event newEvent = new Event
                {
                    Name = model.Name,
                    Description = model.Description,
                    OrganiserId = userId,
                    CreatedOn = DateTime.UtcNow,
                    Start = DateTime.ParseExact(model.Start, "dd/MM/yyyy H:mm", null),
                    End = DateTime.ParseExact(model.End, "dd/MM/yyyy H:mm", null),
                    TypeId = model.TypeId,
                };

                await dbContext.Events.AddAsync(newEvent);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }

        public async Task EditEventAsync(AddEventViewModel model, int id)
        {
            var currentEvent = await dbContext.Events.FindAsync(id);

            if (currentEvent != null)
            {
                currentEvent.Name = model.Name;
                currentEvent.Description = model.Description;
                currentEvent.Start = DateTime.ParseExact(model.Start, "dd/MM/yyyy H:mm", null);
                currentEvent.End = DateTime.ParseExact(model.End, "dd/MM/yyyy H:mm", null);
                currentEvent.TypeId = model.TypeId;

                dbContext.Events.Update(currentEvent);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AllEventViewModel>> GetAllEventsAsync()
        {
            return await dbContext.Events
                .Select(x => new AllEventViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Organiser = x.Organiser.UserName,
                    Start = x.Start,
                    Type = x.Type.Name,
                })
                .ToListAsync();
        }

        public async Task<EventViewModel?> GetEventById(int id)
        {
            return await dbContext.Events
                .Where(e => e.Id == id)
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Start =e.Start.ToString(),
                    End = e.End.ToString(),
                    OrganiserId = e.OrganiserId,
                    TypeId = e.TypeId

                }).FirstOrDefaultAsync();
        }

        public async Task<AddEventViewModel?> GetEventByIdForEdit(int id)
        {
            var types = await dbContext.Types
                .Select(x => new TypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();

            return await dbContext.Events
                .Where(x => x.Id == id)
                .Select(x => new AddEventViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Start = x.Start.ToString("dd/MM/yyyy H:mm"),
                    End = x.End.ToString("dd/MM/yyyy H:mm"),
                    TypeId = x.TypeId,
                    Types = types,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AllEventViewModel>> GetJoinedEventsAsync(string userId)
        {
            return await dbContext.EventsParticipants
                .Where(x => x.HelperId == userId)
                .Select(x => new AllEventViewModel
                {
                    Id = x.Event.Id,
                    Name = x.Event.Name,
                    Organiser = x.Event.Organiser.UserName,
                    Start = x.Event.Start,
                    Type = x.Event.Type.Name,
                })
                .ToListAsync();
        }

        public async Task<AddEventViewModel> GetNewAddEventModelAsync()
        {
            var types = await dbContext.Types
                .Select(x => new TypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();

            var model = new AddEventViewModel
            {
                Types = types,
            };
            return model;
        }

        public async Task JoinEventAsync(string userId, EventViewModel eventToJoin)
        {
            bool alreadyJoined = await dbContext.EventsParticipants
                .AnyAsync(x => x.EventId == eventToJoin.Id && x.HelperId == userId);

            if (alreadyJoined == false)
            {
                var eventParticipant = new EventParticipant
                {
                    EventId = eventToJoin.Id,
                    HelperId = userId,
                };
                await dbContext.EventsParticipants.AddAsync(eventParticipant);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task LeaveEventAsync(string userId, EventViewModel eventToLeave)
        {
            var eventParticipant = dbContext.EventsParticipants
                .FirstOrDefault(x => x.EventId == eventToLeave.Id && x.HelperId == userId);

            if (eventParticipant != null)
            {
                dbContext.EventsParticipants.Remove(eventParticipant);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
