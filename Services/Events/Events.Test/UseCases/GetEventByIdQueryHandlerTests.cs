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
    private readonly GetEventByIdQueryHandler _handler;

    public GetEventByIdQueryHandlerTests()
    {
        _eventsRepository = A.Fake<IEventsRepository>();
        _handler = new GetEventByIdQueryHandler(_eventsRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnEvent_WhenEventExists()
    {
        var eventId = Guid.NewGuid();
        var existingEvent = new Event { Id = eventId, Title = "Test Event" };
        A.CallTo(() => _eventsRepository.GetById(eventId)).Returns(existingEvent);

        var query = new GetEventByIdQuery(eventId);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Event.Should().NotBeNull();
        result.Event.Id.Should().Be(eventId);
        A.CallTo(() => _eventsRepository.GetById(eventId)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_ShouldThrowEventNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = Guid.NewGuid();
        A.CallTo(() => _eventsRepository.GetById(eventId)).Returns(Task.FromResult<Event>(null));

        var query = new GetEventByIdQuery(eventId);
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);
        await act.Should().ThrowAsync<EventNotFoundException>()
            .WithMessage($"Event №:({eventId}) was not found.");
        A.CallTo(() => _eventsRepository.GetById(eventId)).MustHaveHappenedOnceExactly();
    }
}
