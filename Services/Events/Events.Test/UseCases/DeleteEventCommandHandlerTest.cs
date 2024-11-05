using CommonFiles.Interfaces;
using Events.Application.Exceptions;
using Events.Application.UseCases.Commands.DeleteEvent;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using FakeItEasy;
using FluentAssertions;


public class DeleteEventCommandHandlerTests
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteEventCommandHandler _handler;

    public DeleteEventCommandHandlerTests()
    {
        _eventsRepository = A.Fake<IEventsRepository>();
        _unitOfWork = A.Fake<IUnitOfWork>();
        _handler = new DeleteEventCommandHandler(_eventsRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEvent_WhenEventExists()
    {

        var eventId = Guid.NewGuid();
        A.CallTo(() => _eventsRepository.GetById(eventId)).Returns(new Event { Id = eventId });
        A.CallTo(() => _unitOfWork.Save(CancellationToken.None)).Returns(Task.CompletedTask);

        var request = new DeleteEventCommand(eventId);
        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
    }

    [Fact]
    public async Task Handle_ShouldThrowEventNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = Guid.NewGuid();
        A.CallTo(() => _eventsRepository.GetById(eventId)).Returns(Task.FromResult<Event>(null));
        A.CallTo(() => _unitOfWork.Save(CancellationToken.None)).Returns(Task.CompletedTask);

        var request = new DeleteEventCommand(eventId);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<EventNotFoundException>()
            .WithMessage($"Event №:({eventId}) was not found.");
        A.CallTo(() => _eventsRepository.Delete(A<Guid>.Ignored)).MustNotHaveHappened();
    }
}
