using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EntityFrameworkCore.AutoFixture.InMemory;
using FluentAssertions;
using ITSTILoop.Context;
using ITSTILoop.Context.Repositories;
using ITSTILoop.Model;
using ITSTILoop.Services;
using ITSTILoopDTOLibrary;
using Moq;
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
                var participantEndpoint2 = fixture.Create<Uri>();
                // Act
                _participantRepository.CreateParticipant(participantName, participantApiKey, participantEndpoint, participantEndpoint2);
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
                var participantEndpoint2 = fixture.Create<Uri>();
                var amount = fixture.Create<Decimal>();
                var currency = fixture.Create<CurrencyCodes>();
                var participant = _participantRepository.CreateParticipant(participantName, participantApiKey, participantEndpoint, participantEndpoint2);
                // Act
                _participantRepository.FundParticipant(participant.ParticipantId, currency, amount);
                // Assert
                var participantResult = _participantRepository.GetById(participant.ParticipantId);
                participantResult.Accounts.Count.Should().Be(1);
                participantResult.Accounts.First().Settlement.Should().Be(amount);
                participantResult.Accounts.First().AvailableFunds.Should().Be(amount);
            }
        }
        public class TransferRequestTest
        {
            private readonly TransferRequestRepository _tranferRequestRepository;
            private readonly ApplicationDbContext _context;

            public TransferRequestTest()
            {
                var fixture = new Fixture().Customize(new InMemoryContextCustomization
                {
                    AutoCreateDatabase = true,
                    OmitDbSets = true
                });
                _context = fixture.Create<ApplicationDbContext>();

                var myProfile = new AutoMapping();
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
                IMapper mapper = new Mapper(configuration);

                _tranferRequestRepository = new TransferRequestRepository(_context, mapper);
            }

            [Fact]
            public void TestCreateTransferRequest()
            {
                //arrange
                var fixture = new Fixture();
                var transferRequestDTO = fixture.Create<TransferRequestDTO>();
                var partyDTO = fixture.Create<PartyDTO>();
                transferRequestDTO.To.Identifier = partyDTO.PartyIdentifier.Identifier;
                transferRequestDTO.To.PartyIdentifierType = partyDTO.PartyIdentifier.PartyIdentifierType;
                //act
                var result = _tranferRequestRepository.CreateTransferRequest(transferRequestDTO, partyDTO,0);
                //assert
                result.Amount.Should().Be(transferRequestDTO.Amount);
                result.From.Identifier.Should().Be(transferRequestDTO.From.Identifier);
                result.From.PartyIdentifierType.Should().Be(transferRequestDTO.From.PartyIdentifierType);
                result.To.Should().BeEquivalentTo(partyDTO);
                var result2 = _context.TransferRequests.First();
                result2.FirstName.Should().Be(partyDTO.FirstName);
            }

            [Fact]
            public void TestRetrieveTransferRequest()
            {
                //arrange
                var fixture = new Fixture();
                var transferRequestDTO = fixture.Create<TransferRequestDTO>();
                var partyDTO = fixture.Create<PartyDTO>();
                transferRequestDTO.To.Identifier = partyDTO.PartyIdentifier.Identifier;
                transferRequestDTO.To.PartyIdentifierType = partyDTO.PartyIdentifier.PartyIdentifierType;                
                var transferRequestResponseDTOExpected = _tranferRequestRepository.CreateTransferRequest(transferRequestDTO, partyDTO,0);
                //act
                var transferRequestResponseDTOResult = _tranferRequestRepository.RetrieveTransferRequest(transferRequestResponseDTOExpected.TransferId);
                //assert
                transferRequestResponseDTOResult.Should().BeEquivalentTo(transferRequestResponseDTOExpected);
            }

        }

        public class TransactionRepositoryTest
        {
            private readonly TransactionRepository _transactionRepository;
            private readonly ApplicationDbContext _context;

            public TransactionRepositoryTest()
            {
                var fixture = new Fixture().Customize(new InMemoryContextCustomization
                {
                    AutoCreateDatabase = true,
                    OmitDbSets = true
                });
                _context = fixture.Create<ApplicationDbContext>();
                _transactionRepository = new TransactionRepository(_context);
            }


            [Fact]
            public async Task CanMakeTransfer()
            {
                // Arrange
                var guid = Guid.NewGuid();
                var sourceFund = 10000;
                var destFund = sourceFund * 2;
                var transferAmount = sourceFund / 2;
                var fixture = new Fixture().Customize(new AutoMoqCustomization());
                var currency = fixture.Create<CurrencyCodes>();
                var accountSource = new Account() { Currency = currency, Settlement = sourceFund };
                var accountDestination = new Account() { Currency = currency, Settlement = destFund };
                var participantSource = fixture.Create<Participant>();
                var participantDestination = fixture.Create<Participant>();
                participantSource.Accounts = new List<Account>() { accountSource };
                participantDestination.Accounts = new List<Account>() { accountDestination };
                _context.Participants.Add(participantSource);
                _context.Participants.Add(participantDestination);
                _context.SaveChanges();
                // Act
                _transactionRepository.MakeTransfer(participantSource.ParticipantId, participantDestination.ParticipantId, transferAmount, currency, guid);
                // Assert
                _context.Transactions.Count().Should().Be(1);
                var transaction = _context.Transactions.First();
                transaction.From.Should().Be(participantSource.ParticipantId);
                transaction.To.Should().Be(participantDestination.ParticipantId);
                transaction.Amount.Should().Be(transferAmount);
                transaction.TransactionType.Should().Be(TransactionTypes.Transfer);
                accountSource = _context.Participants.Find(participantSource.ParticipantId).Accounts.First();
                accountDestination = _context.Participants.Find(participantDestination.ParticipantId).Accounts.First();
                accountSource.Settlement.Should().Be(sourceFund);
                accountSource.Position.Should().Be(-1 * transferAmount);
                accountDestination.Settlement.Should().Be(destFund);
                accountDestination.Position.Should().Be(transferAmount);

            }
        }
    }
}
