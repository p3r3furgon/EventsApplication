using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.UseCases.Commands.CreateEvent;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;


public class CreateEventCommandHandlerTests
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly CreateEventCommandHandler _handler;

    public CreateEventCommandHandlerTests()
    {
        _eventsRepository = A.Fake<IEventsRepository>();
        _fileService = A.Fake<IFileService>();
        _mapper = A.Fake<IMapper>();
        _handler = new CreateEventCommandHandler(_eventsRepository, _fileService, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldCreateEvent_WhenCommandIsValid()
    {
        var command = new CreateEventCommand(
            Title: "Test Event",
            Description: "event",
            DateTime: DateTime.Now.AddDays(1),
            Place: "TestPlace",
            Category: "TestCategory",
            MaxParticipantNumber: 100,
            Image: A.Dummy<IFormFile>()
        );

        var mappedEvent = new Event { Id = Guid.NewGuid() };
        A.CallTo(() => _fileService.SaveFileAsync(command.Image, A<string[]>.Ignored))
            .Returns("test-image.jpg");
        A.CallTo(() => _mapper.Map<Event>(command)).Returns(mappedEvent);
        A.CallTo(() => _eventsRepository.Create(mappedEvent)).Returns(Task.CompletedTask);
        A.CallTo(() => _mapper.Map<CreateEventResponse>(mappedEvent))
            .Returns(new CreateEventResponse(mappedEvent.Id));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(mappedEvent.Id);
        A.CallTo(() => _fileService.SaveFileAsync(command.Image, A<string[]>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _eventsRepository.Create(mappedEvent))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenCommandIsInvalid()
    {

        var command = new CreateEventCommand(
            Title: "",
            Description: "test event",
            DateTime: DateTime.Now.AddDays(1),
            Place: "Test Place",
            Category: "Test Category",
            MaxParticipantNumber: 10,
            Image: A.Dummy<IFormFile>()
        );

        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
        A.CallTo(() => _fileService.SaveFileAsync(A<IFormFile>.Ignored, A<string[]>.Ignored))
            .MustNotHaveHappened();
        A.CallTo(() => _eventsRepository.Create(A<Event>.Ignored))
            .MustNotHaveHappened();
    }
}

