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
using ITSTILoop.Context.Repositories.Interfaces;

namespace ITSTILoopTest
{
    public class PartyLookupTest
    {
        IHttpClientFactory _clientFactory;
        ILogger<HttpPostClient> _logger;
        string sampleUri = "https://example.com/api/stuff";
        string partyIdentifier = "(555)12345678";
        string partyName = "Mert";
        string partyLastName = "Ozdag";
        string partyBank = "BankA";
        PartyIdentifierDTO queryParty = new PartyIdentifierDTO();
        PartyDTO partyDTO;

        public PartyLookupTest()
        {
            queryParty.Identifier = partyIdentifier;
            queryParty.PartyIdentifierType = PartyIdTypes.MSISDN;
            partyDTO = new PartyDTO() { FirstName = partyName, LastName = partyLastName, PartyIdentifier = new PartyIdentifierDTO() { Identifier = partyIdentifier, PartyIdentifierType = PartyIdTypes.MSISDN}};

            var handler = new Mock<HttpMessageHandler>();
            handler.SetupRequest(HttpMethod.Post, sampleUri, async request =>
            {
                // This setup will only match calls with the expected id
                var json = await request.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PartyIdentifierDTO>(json);
                return (model.Identifier == partyIdentifier && model.PartyIdentifierType == PartyIdTypes.MSISDN);
            }).ReturnsResponse(JsonConvert.SerializeObject(partyDTO), "application/json");
            _clientFactory = handler.CreateClientFactory();
            var loggerMock = new Mock<ILogger<HttpPostClient>>();
            _logger = loggerMock.Object;

        }

        [Fact]
        public async Task TestLookupAsync()
        {
            HttpPostClient postClient = new HttpPostClient(_logger, _clientFactory);
            var result = await postClient.PostAsync<PartyIdentifierDTO, PartyDTO>(queryParty, new Uri(sampleUri));

            Assert.Equal(partyName, result.ResponseContent.FirstName);
            Assert.Equal(partyLastName, result.ResponseContent.LastName);            
            Assert.Equal(HttpPostClientResults.Success, result.Result);
        }

        [Fact]
        public async Task TestPartyLookupAsync()
        {
            //arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Freeze<Mock<IPartyRepository>>().Setup(k => k.GetPartyFromTypeAndId(PartyIdTypes.MSISDN, partyIdentifier)).Returns(new Party() { PartyIdentifier = partyIdentifier });
            fixture.Freeze<Mock<IParticipantRepository>>().Setup(k => k.GetParticipantByName(partyBank)).Returns(new Participant { PartyLookupEndpoint = new Uri(sampleUri) });
            fixture.Freeze<Mock<ILogger<PartyLookupService>>>();
            fixture.Freeze<Mock<IHttpPostClient>>().Setup(k => k.PostAsync<PartyIdentifierDTO, PartyDTO>(It.IsAny<PartyIdentifierDTO>(), It.IsAny<Uri>(), "")).Returns(Task.FromResult(new HttpPostClientResponse<PartyDTO>() { ResponseContent = partyDTO, Result = HttpPostClientResults.Success}));
            var sut = fixture.Create<PartyLookupService>();
            //act
            var result = await sut.FindPartyAsync(PartyIdTypes.MSISDN, partyIdentifier);
            //assert
            Assert.Equal(partyName, result.FoundParty.FirstName);
            Assert.Equal(partyLastName, result.FoundParty.LastName);            
            Assert.Equal(PartyLookupServiceResults.Success, result.Result);
        }
    }
}