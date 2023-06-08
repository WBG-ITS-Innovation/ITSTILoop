using CBDCHubContract.Services;
using FluentAssertions;
using ITSTILoopLibrary.UtilityServices;
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
        public async Task CanCheckBalances()
        {
            //bankbethesda:0x2fb282c433d436675aceb2cb9131718de417f909e14d8ddf9cad185e15cabb7f:0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b
            //bankwheaton:0xfa2168486b678f5f898fe4b0b0043f4f01d89ef4aa539351e1355bc186188518:0x34A816710f52a75F4430674a12E2Ad9De1092e86
            //bankprincegeorge:0xef8f5bd41161c88a547b8b3c10248c6a5f542b3c9608e2645087888f65437749:0x6a82bf493725771AD037DD0cf1ABa956e73C18ff
            // Arrange            
            CBDCBridgeEventWatcherConfig ethereumConfig = new CBDCBridgeEventWatcherConfig() { Address = "0x69282EFd96cd19d747944A9FB358feBaf8c16a2D", RpcEndpoint = "https://cbdcuser:testTEST11@cbdcethereum.azurewebsites.net/", Key = "0xb5cc3ddbd4dacedd91eea5994c6cbbcd02b9b8644f89ef4576ce9bad2ae104be" };

            var loggerMock = new Mock<ILogger<EthereumEventRetriever>>();
            var loggerMock2 = new Mock<ILogger<CBDCHubService>>();
            var ethConfig = new Mock<IOptions<CBDCBridgeEventWatcherConfig>>();
            EthereumEventRetriever eventRetriever = new EthereumEventRetriever(loggerMock.Object);
            CBDCHubService service = new CBDCHubService(loggerMock2.Object, eventRetriever, ethConfig.Object);
            var accounts = new List<string>() { "0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b", "0x34A816710f52a75F4430674a12E2Ad9De1092e86", "0x6a82bf493725771AD037DD0cf1ABa956e73C18ff" };

            // Act
            var result = await service.CheckBalancesAsync(accounts);
            // Assert
            result[accounts[0]].Should().BeGreaterThan(1000);
            result[accounts[1]].Should().BeGreaterThan(1000);
            result[accounts[2]].Should().BeGreaterThan(1000);

        }

        [Fact]
        public async Task CanSettleAccounts()
        {
            //bankbethesda:0x2fb282c433d436675aceb2cb9131718de417f909e14d8ddf9cad185e15cabb7f:0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b
            //bankwheaton:0xfa2168486b678f5f898fe4b0b0043f4f01d89ef4aa539351e1355bc186188518:0x34A816710f52a75F4430674a12E2Ad9De1092e86
            //bankprincegeorge:0xef8f5bd41161c88a547b8b3c10248c6a5f542b3c9608e2645087888f65437749:0x6a82bf493725771AD037DD0cf1ABa956e73C18ff
            // Arrange            
            CBDCBridgeEventWatcherConfig ethereumConfig = new CBDCBridgeEventWatcherConfig() { Address = "0x69282EFd96cd19d747944A9FB358feBaf8c16a2D", RpcEndpoint = "https://cbdcuser:testTEST11@cbdcethereum.azurewebsites.net/", Key = "0xb5cc3ddbd4dacedd91eea5994c6cbbcd02b9b8644f89ef4576ce9bad2ae104be" };
            var loggerMock = new Mock<ILogger<EthereumEventRetriever>>();
            var loggerMock2 = new Mock<ILogger<CBDCHubService>>();
            var option = new Mock<IOptions<CBDCBridgeEventWatcherConfig>>();
            option.Setup(k => k.Value).Returns(ethereumConfig);
            EthereumEventRetriever eventRetriever = new EthereumEventRetriever(loggerMock.Object);
            CBDCHubService service = new CBDCHubService(loggerMock2.Object, eventRetriever, option.Object);
            //var accounts = new List<string>() { "0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b", "0x34A816710f52a75F4430674a12E2Ad9De1092e86", "0x6a82bf493725771AD037DD0cf1ABa956e73C18ff" };
            var accounts = new Dictionary<string, decimal>() { { "0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b", 2 }, { "0x34A816710f52a75F4430674a12E2Ad9De1092e86", -1 }, { "0x6a82bf493725771AD037DD0cf1ABa956e73C18ff", -1} };

            // Act
            await service.SettleAccountsAsync(1, accounts);
            // Assert
        }
    }
}
