using AutoMapper;
using Events.Domain.Models;
using Events.Persistance;
using Events.Persistance.Entities;
using Events.Persistance.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Events.Test.Repositories
{
    public class EventsRepositoryTest
    {
        private readonly IMapper _mapper;

        public EventsRepositoryTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventEntity, Event>().ReverseMap();
                cfg.CreateMap<ParticipantEntity, Participant>().ReverseMap();

            });
            _mapper = config.CreateMapper();
        }

        private async Task<EventsDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new EventsDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();
            if(dbContext.Events.Count() <= 0)
            {
                dbContext.Events.Add(
                    new EventEntity()
                    {
                        Id = Guid.Parse("cee4e534-987d-4017-aa31-714b718a8a3f"),
                        Title = "Event1",
                        Description = "",
                        DateTime = new DateTime(2025, 10, 10, 10, 0, 0),
                        Category = "Category1",
                        Place = "Place1",
                        MaxParticipantNumber = 15,
                        Participants = new List<ParticipantEntity>(),
                        Image = ""
                    });
                dbContext.Events.Add(
                    new EventEntity()
                    {
                        Id = Guid.Parse("6f65bad8-dc10-42b0-bd06-a63155dc874d"),
                        Title = "Event2",
                        Description = "Description2",
                        DateTime = new DateTime(2025, 10, 10, 10, 0, 0),
                        Category = "Category2",
                        Place = "Place2",
                        MaxParticipantNumber = 100,
                        Participants = new List<ParticipantEntity>()
                        { new()
                            {
                                Id = Guid.Parse("f38425c3-9846-4d2e-8e61-3a11008e6165"),
                                UserId = Guid.Parse("4e5268af-45c9-4884-965e-d17454a1367d"),
                                Email = "user@user.user",
                                FirstName = "user",
                                Surname = "user",
                                RegistrationDateTime = DateTime.UtcNow
                            }

                        },
                        Image = "3f02f406-b7f6-496e-8be0-4cefa90b999d.png"
                    });
                await dbContext.SaveChangesAsync();
            }
            return dbContext;
        }

        [Fact]
        public async void EventsRepository_GetById_ReturnsEvent()
        {
            var id = Guid.Parse("6f65bad8-dc10-42b0-bd06-a63155dc874d");
            var dbContext = await GetDbContext();
            var eventsRepository = new EventsRepository(dbContext, _mapper);

            var result = await eventsRepository.GetById(id);

            result.Should().NotBeNull();
            result.Should().BeOfType<Event>();
        }

        [Fact]
        public async void EventsRepository_Get_ReturnsEventsCollection()
        {
            var dbContext = await GetDbContext();
            var eventsRepository = new EventsRepository(dbContext, _mapper);

            var result = await eventsRepository.Get();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<Event>>();
        }

        [Fact]
        public async void EventsRepository_Create_ReturnsGuid()
        {
            var @event = Event.Create("Title1", "Description1", DateTime.UtcNow,
                "Place1", "Category1", 100, new List<Participant>(), "Image1.png");

            var dbContext = await GetDbContext();
            var eventsRepository = new EventsRepository(dbContext, _mapper);

            var result = await eventsRepository.Create(@event);
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async void EventsRepository_Delete_ReturnsEvent()
        {
            var id = Guid.Parse("6f65bad8-dc10-42b0-bd06-a63155dc874d");
            var dbContext = await GetDbContext();
            var eventsRepository = new EventsRepository(dbContext, _mapper);

            var result = await eventsRepository.Delete(id);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async void EventsRepository_Update_ReturnsEvent()
        {
            var id = Guid.Parse("cee4e534-987d-4017-aa31-714b718a8a3f");
            var dbContext = await GetDbContext();
            var eventsRepository = new EventsRepository(dbContext, _mapper);

            var result = await eventsRepository.Update(id, "title","descr", 
                new DateTime(2025,1,1,0,0,0),"category", "place", 20, "image.png");

            result.Should().NotBeEmpty();
        }

    }
}
