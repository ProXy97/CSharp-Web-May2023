using Homies.Interfaces;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public async Task<IActionResult> All()
        {
            var model = await eventService.GetAllEventsAsync();
            return View(model);
        }

        public async Task<IActionResult> Joined()
        {
            var model = await eventService.GetJoinedEventsAsync(GetUserId());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
           
            AddEventViewModel model = await eventService.GetNewAddEventModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            if (DateTime.Parse(model.Start) < DateTime.Parse(model.End))
            {

            }

            await eventService.AddEventAsync(model, GetUserId());

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AddEventViewModel? model = await eventService.GetEventByIdForEdit(id);
            
            if(model == null)
            {
                  return RedirectToAction(nameof(All));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(AddEventViewModel model, int id)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await eventService.EditEventAsync(model, id);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Join(int id)
        {
            var @event = await eventService.GetEventById(id);

            if (@event == null)
            {
                return RedirectToAction(nameof(All));
            }
            var userId = GetUserId();

            await eventService.JoinEventAsync(userId, @event);

            return RedirectToAction(nameof(Joined));
        }


        public async Task<IActionResult> Leave(int id)
        {
            var @event = await eventService.GetEventById(id);

            if (@event == null)
            {
                return RedirectToAction(nameof(All));
            }
            var userId = GetUserId();

            await eventService.LeaveEventAsync(userId, @event);

            return RedirectToAction(nameof(All));
        }
        protected string GetUserId()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }


    }
}
