using CBDCHubContract.Services;
using Microsoft.Extensions.Logging;
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
            // Arrange            
            EthereumConfig ethereumConfig = new EthereumConfig();
            var loggerMock = new Mock<ILogger<EthereumEventRetriever>>();
            var loggerMock2 = new Mock<ILogger<CBDCBridgeService>>();
            //EthereumEventRetriever eventRetriever = new EthereumEventRetriever(loggerMock.Object);
            //CBDCBridgeService service = new CBDCBridgeService(loggerMock2.Object, eventRetriever);

            // Act
            //var result = await service.GetRegisteredFSPAsync();

            // Assert
            
        }
    }
}
