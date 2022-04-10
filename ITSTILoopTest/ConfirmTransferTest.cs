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
using AutoFixture;
using ITSTILoop.Model;
using AutoFixture.AutoMoq;
using ITSTILoop.Services.Interfaces;
using FluentAssertions;
using ITSTILoop.Context.Repositories.Interfaces;

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
        public async Task TestConfirmTransferAsync()
        {
            //arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Freeze<Mock<IParticipantRepository>>().Setup(k => k.GetParticipantByName(partyBank)).Returns(new Participant { PartyLookupEndpoint = new Uri(sampleUri), ConfirmTransferEndpoint = new Uri(sampleUri) });
            fixture.Freeze<Mock<ILogger<ConfirmTransferService>>>();
            var tranferRequestResponseDTO = fixture.Create<TransferRequestResponseDTO>();
            var confirmResult = fixture.Create<HttpPostClientResponse<TransferRequestCompleteDTO>>();
            //LookupPartyAsync(It.IsAny<PartyIdentifierDTO>(), It.IsAny<Uri>())).Returns(Task.FromResult(new PartyLookupServiceResult() { FoundParty = partyDTO, Result = PartyLookupServiceResults.Success }));
            fixture.Freeze<Mock<IHttpPostClient>>().Setup(k => k.PostAsync<TransferRequestResponseDTO, TransferRequestCompleteDTO>(It.IsAny<TransferRequestResponseDTO>(), It.IsAny<Uri>(), "")).Returns(Task.FromResult(confirmResult));
            var sut = fixture.Create<ConfirmTransferService>();
            //act
            var result = await sut.ConfirmTransferAsync(tranferRequestResponseDTO);
            //assert
            result.Result.Should().Be(ParticipantConfirmTransferServiceResults.Success);
            result.TransferRequestComplete.Should().BeEquivalentTo(confirmResult.ResponseContent);
        }
    }
}
