using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.InMemory;
using FluentAssertions;
using ITSTILoop.Context;
using ITSTILoop.Context.Repositories;
using ITSTILoop.Model;
using ITSTILoopDTOLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITSTILoopTest
{
    public class RepositoryTest
    {
        public readonly ApplicationDbContext _context;

        public RepositoryTest()
        {
            var fixture = new Fixture().Customize(new InMemoryContextCustomization
            {
                AutoCreateDatabase = true,
                OmitDbSets = true
            });
            _context = fixture.Create<ApplicationDbContext>();
        }


        [Fact]
        public async Task CanCreateSettlementWindow()
        {
            // Arrange            
            SettlementWindowRepository settlementWindowRepository = new SettlementWindowRepository(_context);

            // Act
            settlementWindowRepository.CreateNewSettlementWindow();

            // Assert
            _context.SettlementWindows.Should().HaveCount(1);
        }

        public class ParticipantRepositoryTest
        {
            private readonly ParticipantRepository _participantRepository;

            public ParticipantRepositoryTest()
            {
                var fixture = new Fixture().Customize(new InMemoryContextCustomization
                {
                    AutoCreateDatabase = true,
                    OmitDbSets = true
                });
                var context = fixture.Create<ApplicationDbContext>();
                _participantRepository = new ParticipantRepository(context);
            }

            [Fact]
            public async Task CanCreateParticipant()
            {
                // Arrange
                var fixture = new Fixture().Customize(new AutoMoqCustomization());
                var participantName = fixture.Create<string>();
                var participantApiKey = fixture.Create<string>();
                var participantEndpoint = fixture.Create<Uri>();
                // Act
                _participantRepository.CreateParticipant(participantName, participantApiKey, participantEndpoint);
                // Assert
                var participantResult = _participantRepository.Find(k => k.Name == participantName).First();
                participantResult.Should().NotBeNull();
                participantResult.Name.Should().Be(participantName);
            }


            [Fact]
            public async Task CanFundParticipant()
            {
                // Arrange
                var fixture = new Fixture().Customize(new AutoMoqCustomization());
                var participantName = fixture.Create<string>();
                var participantApiKey = fixture.Create<string>();
                var participantEndpoint = fixture.Create<Uri>();                
                var amount = fixture.Create<Decimal>();
                var currency = fixture.Create<CurrencyCodes>();
                var participant = _participantRepository.CreateParticipant(participantName, participantApiKey, participantEndpoint);
                // Act
                _participantRepository.FundParticipant(participant.ParticipantId, currency, amount);
                // Assert
                var participantResult = _participantRepository.GetById(participant.ParticipantId);
                participantResult.Accounts.Count.Should().Be(1);
                participantResult.Accounts.First().Settlement.Should().Be(amount);
                participantResult.Accounts.First().AvailableFunds.Should().Be(amount);
            }
        }
    }
}
