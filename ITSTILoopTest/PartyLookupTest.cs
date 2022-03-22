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

namespace ITSTILoopTest
{
    public class PartyLookupTest
    {
        IHttpClientFactory _clientFactory;
        ILogger<PartyLookupService> _logger;
        string sampleUri = "https://example.com/api/stuff";
        string partyIdentifier = "(555)12345678";
        string partyName = "Mert";
        string partyLastName = "Ozdag";
        string partyBank = "BankA";
        QueryPartyDTO queryParty = new QueryPartyDTO();


        public PartyLookupTest()
        {
            queryParty.PartyIdentifier = partyIdentifier;
            queryParty.PartyIdentifierType = PartyIdTypes.MSISDN;
            PartyDTO partyDTO = new PartyDTO() { FirstName = partyName, LastName = partyLastName, PartyIdentifierType = PartyIdTypes.MSISDN, PartyIdentifier = partyIdentifier, RegisteredParticipantName = partyBank } ;

            var handler = new Mock<HttpMessageHandler>();
            handler.SetupRequest(HttpMethod.Post, sampleUri, async request =>
            {
                // This setup will only match calls with the expected id
                var json = await request.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<QueryPartyDTO>(json);
                return (model.PartyIdentifier == partyIdentifier && model.PartyIdentifierType == PartyIdTypes.MSISDN);
            }).ReturnsResponse(JsonConvert.SerializeObject(partyDTO), "application/json");
            _clientFactory = handler.CreateClientFactory();
            var loggerMock = new Mock<ILogger<PartyLookupService>>();
            _logger = loggerMock.Object;

        }

        [Fact]
        public async Task TestLookupAsync()
        {
            PartyLookupService partyLookupService = new PartyLookupService(_logger, _clientFactory);
            var result = await partyLookupService.LookupPartyAsync(queryParty, new Uri(sampleUri));
            
            Assert.Equal(partyName, result.FoundParty.FirstName);
            Assert.Equal(partyLastName, result.FoundParty.LastName);
            Assert.Equal(partyBank, result.FoundParty.RegisteredParticipantName);
            Assert.Equal(PartyLookupServiceResults.Success, result.Result);
        }
    }
}