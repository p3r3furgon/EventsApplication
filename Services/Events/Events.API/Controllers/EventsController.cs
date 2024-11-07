using CommonFiles.Pagination;
using Events.Application.Dtos;
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
        public async Task<IActionResult> CreateEvent([FromForm] EventRequestDto eventDto)
        {
            var response = await _mediator.Send(new CreateEventCommand(eventDto));
            return StatusCode(StatusCodes.Status201Created, response);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromForm] EventRequestDto eventDto,
            string? messageTitle, string? messageContent)
        {
            var response = await _mediator.Send(new UpdateEventCommand(id, eventDto, messageTitle, messageContent));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var response = await _mediator.Send(new DeleteEventCommand(id));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("{id}/subscribe")]
        [Authorize]
        public  async Task<IActionResult> RegisterUserOnEvent(Guid id)
        {
            var firstName = User?.FindFirstValue(ClaimTypes.Name);
            var surname = User?.FindFirstValue(ClaimTypes.Surname);
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            var response = await _mediator.Send(new SubscribeOnEventCommand(id, firstName, surname, email, Guid.Parse(userId)));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{id}/unsubscribe")]
        [Authorize]
        public async Task<IActionResult> UnsubscribeFromEvent(Guid id)
        {
            var userId = User?.FindFirstValue(ClaimTypes.PrimarySid);

            var response = await _mediator.Send(new UnsubscribeFromEventCommand(id, Guid.Parse(userId)));
            return StatusCode(StatusCodes.Status200OK, response);
        }


    }
}
