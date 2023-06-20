using Homies.Models;

namespace Homies.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<AllEventViewModel>> GetAllEventsAsync();

        Task<IEnumerable<AllEventViewModel>> GetJoinedEventsAsync(string userId);

        Task<AddEventViewModel> GetNewAddEventModelAsync();

        Task AddEventAsync(AddEventViewModel model, string userId);

        Task EditEventAsync(AddEventViewModel model, int id);

        Task<AddEventViewModel?> GetEventByIdForEdit(int id);

        Task<EventViewModel?> GetEventById(int id);

        Task LeaveEventAsync(string userId, EventViewModel eventToLeave);

        Task JoinEventAsync(string userId, EventViewModel eventToJoin);

    }
}
