using Events.Persistance.Repositories;
using Events.Persistance;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Models;
using FluentAssertions;

public class EventsRepositoryTests
{
    private readonly EventsDbContext _context;
    private readonly EventsRepository _repository;

    public EventsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<EventsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EventsDbContext(options);
        _repository = new EventsRepository(_context);
    }

    [Fact]
    public async Task Create_ShouldAddEventToDatabase()
    {
        var @event = Event.Create(
            title: "TestEvent",
            description: "TestDescr",
            dateTime: DateTime.Now.AddDays(1),
            place: "TestPlace",
            category: "",
            maxParticipantNumber: 10,
            image: "image.jpg");

        await _repository.Create(@event);

        var addedEvent = await _context.Events.FindAsync(@event.Id);
        addedEvent.Should().NotBeNull();
        addedEvent.Title.Should().Be("TestEvent");
    }

    [Fact]
    public async Task GetById_ShouldReturnEventWithParticipants()
    {
        var @event = Event.Create(
            title: "TestEvent",
            description: "TestDescr",
            dateTime: DateTime.Now.AddDays(1),
            place: "TestPlace",
            category: "",
            maxParticipantNumber: 10,
            image: "image.jpg");

        var participant = Participant.Create(
            userId: Guid.NewGuid(),
            firstName: "Dmitry",
            surname: "Admin",
            email: "admin@gmail.com",
            registrationDateTime: DateTime.Now);

        @event.Participants.Add(participant);
        await _repository.Create(@event);

        var result = await _repository.GetById(@event.Id);

        result.Should().NotBeNull();
        result!.Title.Should().Be("TestEvent");
        result.Participants.Should().ContainSingle();
    }

    [Fact]
    public async Task Delete_ShouldRemoveEventFromDatabase()
    {
        var @event = Event.Create(
            title: "TestEvent",
            description: "TestDescr",
            dateTime: DateTime.Now.AddDays(1),
            place: "TestPlace",
            category: "",
            maxParticipantNumber: 10,
            image: "image.jpg");

        await _repository.Create(@event);

        await _repository.Delete(@event.Id);
        var deletedEvent = await _context.Events.FindAsync(@event.Id);
        deletedEvent.Should().BeNull();
    }
}
