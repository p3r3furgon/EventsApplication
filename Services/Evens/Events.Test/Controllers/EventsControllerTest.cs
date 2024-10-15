using CommonFiles.Messaging;
using Events.API.Controllers;
using Events.API.Dtos;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using FakeItEasy;
using FluentAssertions;
using Gridify;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Event = Events.Domain.Models.Event;

namespace Events.Test.Controllers
{
    public class EventsControllerTest
    {
        private readonly IEventsService _eventsService;
        private readonly IFileService _fileService;
        private readonly IPublishEndpoint _publishEndpoint;

        public EventsControllerTest()
        {
            _eventsService = A.Fake<IEventsService>();
            _fileService = A.Fake<IFileService>();
            _publishEndpoint = A.Fake<IPublishEndpoint>();
        }

        [Fact]
        public async Task EventsController_GetEvents_ReturnStatusCode200Async()
        {
            var events = new List<Event>
            {
                Event.Create("Title1", "Description1", DateTime.UtcNow,
                "Place1", "Category1", 100, new List<Participant>(), "Image1.png"),
                Event.Create("Title2", "", DateTime.UtcNow,
                "Place2", "", 100, new List<Participant>(), ""),
            };
            A.CallTo(() => _eventsService.GetEvents()).Returns(events);

            var controller = new EventsController(_eventsService, _fileService, _publishEndpoint);

            var gridifyQuery = new GridifyQuery();
            gridifyQuery.Page = 1;
            gridifyQuery.PageSize = 5;

            var result = await controller.GetEvents(gridifyQuery) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedEvents = (result.Value as IQueryable<EventResponceDto>).ToList();
            returnedEvents.Should().NotBeNull();
            returnedEvents.Count().Should().Be(2);

        }

        [Fact]
        public async Task EventsController_CreateEvent_Return201Created()
        {
            var controller = new EventsController(_eventsService, _fileService, _publishEndpoint);

            var createEventRequest = new CreateEventRequest
            {
                Title = "New Event",
                Description = "Event Description",
                DateTime = DateTime.Parse("2025-12-20T10:00:00Z"),
                Place = "Location",
                Category = "Category",
                MaxparticipantNumber = 100,
                Image = new FormFile(new MemoryStream(new byte[0]), 0, 0, "file", "image.jpg")
            };

            string expectedImageName = "image.jpg";
            A.CallTo(() => _fileService.SaveFileAsync(createEventRequest.Image, A<string[]>.Ignored))
                .Returns(expectedImageName);
            Guid expectedEventId = Guid.NewGuid();
            A.CallTo(() => _eventsService.CreateEvent(A<Event>.Ignored)).Returns(Task.FromResult(expectedEventId));

            var result = await controller.CreateEvent(createEventRequest) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().Be(expectedEventId);
        }

        [Fact]
        public async Task EventsController_CreateEvent_Return400BadRequest()
        {
            var controller = new EventsController(_eventsService, _fileService, _publishEndpoint);

            var createEventRequest = new CreateEventRequest
            {
                // title = "" не пройдет через валидатор
                Title = string.Empty,
                Description = "Event Description",
                DateTime = DateTime.Now,
                Place = "Location",
                Category = "Category",
                MaxparticipantNumber = 100,
                Image = null
            };

            var result = await controller.CreateEvent(createEventRequest) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().NotBeNull();
        }

    }
}
