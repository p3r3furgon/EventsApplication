using AutoMapper;
using CommonFiles.Interfaces;
using Events.Application.Dtos;
using Events.Application.Exceptions;
using Events.Application.UseCases.Queries.GetEventById;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using FakeItEasy;
using FluentAssertions;
using Xunit;

public class GetEventByIdQueryHandlerTests
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IMapper _mapper;
    private readonly GetEventByIdQueryHandler _handler;

    public GetEventByIdQueryHandlerTests()
    {
        _eventsRepository = A.Fake<IEventsRepository>();
        _mapper = A.Fake<IMapper>();
        _handler = new GetEventByIdQueryHandler(_eventsRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnEvent_WhenEventExists()
    {
        var eventId = Guid.NewGuid();
        var existingEvent = new Event { Id = eventId, Title = "Test Event" };
        var eventDto = new EventResponseDto { Id = eventId, Title = "Test Event" };

        A.CallTo(() => _eventsRepository.GetById(eventId)).Returns(existingEvent);
        A.CallTo(() => _mapper.Map<EventResponseDto>(existingEvent)).Returns(eventDto);

        var request = new GetEventByIdQuery(eventId);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Event.Should().NotBeNull();
        result.Event.Id.Should().Be(eventId);
        result.Event.Title.Should().Be("Test Event");
        A.CallTo(() => _eventsRepository.GetById(eventId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.Map<EventResponseDto>(existingEvent)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_ShouldThrowEventNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = Guid.NewGuid();
        A.CallTo(() => _eventsRepository.GetById(eventId)).Returns(Task.FromResult<Event>(null));

        var request = new GetEventByIdQuery(eventId);
        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<EventNotFoundException>()
            .WithMessage($"Event №:({eventId}) was not found.");
        A.CallTo(() => _eventsRepository.GetById(eventId)).MustHaveHappenedOnceExactly();
    }
}
