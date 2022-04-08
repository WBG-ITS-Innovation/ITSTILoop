using ITSTILoop.Services;
using ITSTILoopDTOLibrary;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System;
using ITSTILoop.Context;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.InMemory;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Model;
using AutoFixture.AutoMoq;
using ITSTILoop.Services.Interfaces;
using FluentAssertions;

namespace ITSTILoopTest
{
    public class ConfirmTransferTest
    {
        string sampleUri = "https://example.com/api/stuff";
        string partyIdentifier = "(555)12345678";
        string partyName = "Mert";
        string partyLastName = "Ozdag";
        string partyBank = "BankA";


        [Fact]
        public async Task TestParticipantTransferAsync()
        {
            //arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());            
            var logger = new Mock<ILogger<ParticipantConfirmTransferService>>();
            var handler = new Mock<HttpMessageHandler>();
            var transferCompleteResponse = fixture.Create<TransferRequestCompleteDTO>();
            var transfer = fixture.Create<TransferRequestResponseDTO>();
            transfer.To.FirstName = partyName;
            handler.SetupRequest(HttpMethod.Post, sampleUri, async request =>
            {
                // This setup will only match calls with the expected id
                var json = await request.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<TransferRequestResponseDTO>(json);
                return (model.To.FirstName == partyName);
            }).ReturnsResponse(JsonConvert.SerializeObject(transferCompleteResponse), "application/json");
            var clientFactory = handler.CreateClientFactory();
            var sut = new ParticipantConfirmTransferService(logger.Object, clientFactory);
            //act
            var result = await sut.ConfirmTransferAsync(transfer, new Uri(sampleUri));
            //assert
            result.TransferRequestComplete.Should().BeEquivalentTo(transferCompleteResponse);
        }


        [Fact]
        public async Task TestConfirmTransferAsync()
        {
            //arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Freeze<Mock<IParticipantRepository>>().Setup(k => k.GetParticipantByName(partyBank)).Returns(new Participant { PartyLookupEndpoint = new Uri(sampleUri), ConfirmTransferEndpoint = new Uri(sampleUri) });
            fixture.Freeze<Mock<ILogger<ConfirmTransferService>>>();
            var tranferRequestResponseDTO = fixture.Create<TransferRequestResponseDTO>();
            var confirmResult = fixture.Create<ParticipantConfirmTransferServiceResult>();
            //LookupPartyAsync(It.IsAny<PartyIdentifierDTO>(), It.IsAny<Uri>())).Returns(Task.FromResult(new PartyLookupServiceResult() { FoundParty = partyDTO, Result = PartyLookupServiceResults.Success }));
            fixture.Freeze<Mock<IParticipantConfirmTransferService>>().Setup(k => k.ConfirmTransferAsync(It.IsAny<TransferRequestResponseDTO>(), It.IsAny<Uri>())).Returns(Task.FromResult(confirmResult));
            var sut = fixture.Create<ConfirmTransferService>();
            //act
            var result = await sut.ConfirmTransferAsync(tranferRequestResponseDTO);
            //assert
            result.Result.Should().Be(ParticipantConfirmTransferServiceResults.Success);
            result.TransferRequestComplete.Should().BeEquivalentTo(confirmResult.TransferRequestComplete);
        }
    }
}
