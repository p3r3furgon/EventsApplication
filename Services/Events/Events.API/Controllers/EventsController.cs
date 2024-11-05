using CommonFiles.Pagination;
using Events.Application.UseCases.Commands.CreateEvent;
using Events.Application.UseCases.Commands.DeleteEvent;
using Events.Application.UseCases.Commands.SubscribeOnEvent;
using Events.Application.UseCases.Commands.UnsubscribeFromEvent;
using Events.Application.UseCases.Commands.UpdateEvent;
using Events.Application.UseCases.Queries.GetEventById;
using Events.Application.UseCases.Queries.GetEventParticipants;
using Events.Application.UseCases.Queries.GetEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] PaginationParams paginationParams, string? filter)
        {

            var response = await _mediator.Send(new GetEventsQuery(paginationParams, filter));

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var response = await _mediator.Send(new GetEventByIdQuery(id));
            return StatusCode(StatusCodes.Status200OK, response.Event);
        }

        [HttpGet("{id}/paricipants")]
        [Authorize]
        public async Task<IActionResult> GetEventParticipants(Guid id, [FromQuery] PaginationParams paginationParams)
        {

            var response = await _mediator.Send(new GetEventParticipantsQuery(id, paginationParams));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventCommand createEventCommand)
        {
            var response = await _mediator.Send(createEventCommand);
            return StatusCode(StatusCodes.Status201Created, response);
        }


        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventCommand updateEventCommand)
        {
            var response = await _mediator.Send(updateEventCommand);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> DeleteEvent(DeleteEventCommand deleteEventCommand)
        {
            var response = await _mediator.Send(deleteEventCommand);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("subscribe")]
        [Authorize]
        public  async Task<IActionResult> RegisterUserOnEvent(Guid eventId)
        {
            var firstName = User?.FindFirstValue(ClaimTypes.Name);
            var surname = User?.FindFirstValue(ClaimTypes.Surname);
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            var response = await _mediator.Send(new SubscribeOnEventCommand(eventId, firstName, surname, email, Guid.Parse(userId)));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("unsubscribe")]
        [Authorize]
        public async Task<IActionResult> UnsubscribeFromEvent(Guid eventId)
        {
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            var response = await _mediator.Send(new UnsubscribeFromEventCommand(eventId, Guid.Parse(userId)));
            return StatusCode(StatusCodes.Status200OK, response);
        }


    }
}
