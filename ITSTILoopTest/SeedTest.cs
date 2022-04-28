using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using EntityFrameworkCore.AutoFixture.InMemory;
using FluentAssertions;
using ITSTILoop.Context;
using ITSTILoop.Context.Repositories;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Helpers;
using ITSTILoop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ITSTILoopTest
{
	public class SeedTest
	{
        public readonly ApplicationDbContext _context;

        public SeedTest()
        {
            var fixture = new Fixture().Customize(new InMemoryContextCustomization
            {
                AutoCreateDatabase = true,
                OmitDbSets = true
            });
            _context = fixture.Create<ApplicationDbContext>();
        }

        [Fact]
        public void TestSeedFsp()
        {
            //arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var name = fixture.Create<string>();
            var apiId = fixture.Create<string>();
            var apiKey = fixture.Create<string>();
            var partyIdentifier1 = fixture.Create<string>();
            var partyIdentifier2 = fixture.Create<string>();
            var cdbcAddress = fixture.Create<string>();

            var participantText = $"{name}|{apiId}|{apiKey}|{cdbcAddress}";
            var partiesText = $"{partyIdentifier1}|{partyIdentifier2}";

            var participantRepository = new ParticipantRepository(_context);
            var loggerMock = new Mock<ILogger<SampleFspSeedingService>>();
            var settlementMon = new Mock<ISettlementWindowRepository>();
            var sut = new SampleFspSeedingService(loggerMock.Object, participantRepository, settlementMon.Object);
            //act
            sut.SeedFsp(participantText, partiesText);
            //assert
            var participant = _context.Participants.Include(k => k.Parties).FirstOrDefault();
            participant.Name.Should().Be(name);
            participant.ApiKey.Should().Be(apiKey);
            participant.ApiId.Should().Be(apiId);
            participant.CBDCAddress.Should().Be(cdbcAddress);
            participant.Parties.Count.Should().Be(2);
            participant.Parties.ElementAt(0).PartyIdentifier.Should().Be(partyIdentifier1);
            participant.Parties.ElementAt(1).PartyIdentifier.Should().Be(partyIdentifier2);
        }

    }
}

