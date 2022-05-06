using CBDCHubContract.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITSTILoopTest
{
    public class CBDCBridgeTest
    {
        //TODO: Fix this test
        [Fact]
        public async Task CanRetrieveLogs()
        {
            //bankbethesda:0x2fb282c433d436675aceb2cb9131718de417f909e14d8ddf9cad185e15cabb7f:0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b
            //bankwheaton:0xfa2168486b678f5f898fe4b0b0043f4f01d89ef4aa539351e1355bc186188518:0x34A816710f52a75F4430674a12E2Ad9De1092e86
            //bankprincegeorge:0xef8f5bd41161c88a547b8b3c10248c6a5f542b3c9608e2645087888f65437749:0x6a82bf493725771AD037DD0cf1ABa956e73C18ff
            // Arrange            
            EthereumConfig ethereumConfig = new EthereumConfig() { ContractAddress = "0x5900F92b0Db9a0616DD2AC1353960994127B7719", RpcEndpoint = "https://u0covh2ejy:Y0nM-MyycOwLdmJpejQOLQPtBv4pcpNVtlIJzOYuuzc@u0lfqhwrcv-u0cpx68j0x-rpc.us0-aws.kaleido.io/", ContractOwnerKey = "c51802b6b7ac9fd7eff24243835023060068439916f8ff15ec9b81ca87522708" };
            var loggerMock = new Mock<ILogger<EthereumEventRetriever>>();
            var loggerMock2 = new Mock<ILogger<CBDCBridgeService>>();
            var option = new Mock<IOptions<EthereumConfig>>();
            option.Setup(k => k.Value).Returns(ethereumConfig);
            EthereumEventRetriever eventRetriever = new EthereumEventRetriever(loggerMock.Object, option.Object);
            CBDCBridgeService service = new CBDCBridgeService(loggerMock2.Object, eventRetriever);

            // Act
            var result = await service.GetRegisteredFSPAsync();

            // Assert
            
        }
    }
}
