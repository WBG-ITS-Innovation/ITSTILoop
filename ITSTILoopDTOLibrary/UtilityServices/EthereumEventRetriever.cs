using System.Numerics;
using ITSTILoopLibrary.Utility;
using Microsoft.Extensions.Logging;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace ITSTILoopLibrary.UtilityServices
{
    public class EthereumConfig
    {
        public string ContractAddress { get; set; } = String.Empty;
        public string ContractOwnerKey { get; set; } = String.Empty;
        public string ContractTransactionHash { get; set; } = String.Empty;
        public string RpcEndpoint { get; set; } = String.Empty;
        public int NetworkId { get; set; } = 0;
    }

    public class EthereumEventRetriever
    {
        private readonly ILogger<EthereumEventRetriever> _logger;
        private Web3? _web3 = null;
        private EthereumConfig? _config;

        public EthereumConfig? Config
        {
            get
            {
                return _config;
            }
            set
            {
                if (value != null)
                {
                    _config = value;
                    IClient client = new RpcClient(new Uri(_config.RpcEndpoint));
                    _web3 = new Web3(client);
                }
            }
        }

        public EthereumEventRetriever(ILogger<EthereumEventRetriever> logger)
        {
            _logger = logger;

        }

        public Event<T>? CreateEventHandler<T>() where T : IEventDTO, new()
        {
            if (_web3 != null && _config != null)
            {
                return _web3.Eth.GetEvent<T>(_config.ContractAddress);
            }
            else
            {
                return null;
            }
        }

        public Event<T>? CreateEventHandler<T>(string contractAddress) where T : IEventDTO, new()
        {
            if (_web3 != null)
            {
                return _web3.Eth.GetEvent<T>(contractAddress);
            }
            else return null;
        }

        public async Task<BigInteger> GetLatestBlockAsync()
        {
            if (_web3 != null)
            {
                BigInteger latestBlock = 0;
                try
                {
                    var t = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                    latestBlock = t.Value;
                    _logger.LogInformation($"GetLatestBlockAsync-LatestBlock:{latestBlock}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetLatestBlockAsync-GetBlockNumberException:{ex}");
                }
                return latestBlock;
            }
            else
            {
                return -1;
            }
        }

        public async Task RetrievePastLogsAsync<T>(Event<T> eventLog, Func<List<EventLog<T>>, Task> logProcessor, BlockParameter fromBlockParameter, BlockParameter toBlockParameter) where T : IEventDTO, new()
        {
            var filterAll = eventLog.CreateFilterInput(fromBlockParameter, toBlockParameter);
            var eventLogs = await eventLog.GetAllChangesAsync(filterAll);
            if (eventLogs.Count > 0)
            {
                await logProcessor(eventLogs);
            }
        }

        public async Task<BigInteger> RetrievePastLogsAsync<T>(Event<T> eventLog, Func<List<EventLog<T>>, Task> logProcessor) where T : IEventDTO, new()
        {
            //get the block
            if (_web3 != null && _config != null)
            {
                var transaction = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(_config.ContractTransactionHash);
                return await RetrievePastLogsAsync<T>(eventLog, logProcessor, transaction.BlockNumber);
            }
            return -1;
        }

        public async Task<BigInteger> RetrievePastLogsAsync<T>(Event<T> eventLog, Func<List<EventLog<T>>, Task> logProcessor, BigInteger startFromBlock) where T : IEventDTO, new()
        {

            _logger.LogInformation("RetrievePastLogs-ENTRY");
            if (_web3 != null)
            {
                BigInteger latestBlock = 0;
                BigInteger fromBlock = startFromBlock;
                int increment = 15000;
                latestBlock = await GetLatestBlockAsync();
                int successCount = 0;
                while (true)
                {
                    if (latestBlock > 0 && latestBlock > fromBlock)
                    {
                        try
                        {
                            BlockParameter fromBlockParameter = new BlockParameter(new Nethereum.Hex.HexTypes.HexBigInteger(fromBlock));
                            BlockParameter toBlockParameter = new BlockParameter(new Nethereum.Hex.HexTypes.HexBigInteger(fromBlock + increment));
                            //logs
                            var filterAll = eventLog.CreateFilterInput(fromBlockParameter, toBlockParameter);
                            _logger.LogInformation($"from {fromBlock} to {fromBlock + increment} {typeof(T)} logs. Increment:{increment}");
                            var eventLogs = await eventLog.GetAllChangesAsync(filterAll);
                            if (eventLogs.Count > 0)
                            {
                                await logProcessor(eventLogs);
                            }
                            fromBlock = fromBlock + increment;
                            if (increment < 15000 && successCount > 10)
                            {
                                increment = increment + 1500;
                            }
                            successCount++;
                        }
                        catch (RpcClientTimeoutException)
                        {
                            _logger.LogError("RpcClientTimeoutException");
                            successCount = 0;
                            await Task.Delay(1000);
                            if (increment > 1000) increment = increment / 5;
                        }
                        catch (RpcClientUnknownException)
                        {
                            _logger.LogError("RpcClientUnknownException");
                            successCount = 0;
                            await Task.Delay(1000);
                            if (increment > 1000) increment = increment / 5;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"{ex}");
                        }
                    }
                    else
                    {
                        break;
                    }
                    await Task.Delay(800);
                }
                _logger.LogInformation("RetrievePastLogs-EXIT");
                return latestBlock;
            }
            else
            {
                _logger.LogError("RetrievePastLogs-ERROR-Web3-Null");
                return -1;
            }
        }

        public async Task GetEventLogsBetweenBlocksAsync<T>(Event<T> eventHandler, BlockParameter startBlock, BlockParameter endBlock, Func<List<EventLog<T>>, Task> logProcessor) where T : IEventDTO, new()
        {
            var filterAllEventsForContract = eventHandler.CreateFilterInput(startBlock, endBlock);
            var projectEventLogs = await eventHandler.GetAllChangesAsync(filterAllEventsForContract);
            await logProcessor(projectEventLogs);
        }

        public async Task CheckForNewLogsAsync(Func<BlockParameter, BlockParameter, Task> blockHandler, BigInteger startBlock, CancellationToken stoppingToken)
        {
            _logger.LogInformation("CheckForNewLogs-ENTRY");
            if (_web3 != null)
            {
                BigInteger latestBlock = 0;
                BigInteger fromBlock = 0;
                BigInteger lastBlock = 0;

                while (!stoppingToken.IsCancellationRequested)
                {
                    latestBlock = await GetLatestBlockAsync();
                    if (latestBlock > 0 && (latestBlock > fromBlock || lastBlock != latestBlock))
                    {
                        if (fromBlock == 0)
                        {
                            fromBlock = startBlock;
                        }

                        try
                        {
                            BlockParameter fromBlockParameter = new BlockParameter(new Nethereum.Hex.HexTypes.HexBigInteger(fromBlock));
                            BlockParameter toBlockParameter = new BlockParameter(new Nethereum.Hex.HexTypes.HexBigInteger(latestBlock));
                            _logger.LogInformation($"CheckForNewLogs-{fromBlock}-{latestBlock}");
                            await blockHandler(fromBlockParameter, toBlockParameter);
                            fromBlock = latestBlock + 1;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"CheckForNewLogs-InnerException:{ex}");
                        }

                    }
                    lastBlock = latestBlock;
                    await Task.Delay(5000, stoppingToken);
                }
            }
            else
            {
                _logger.LogError($"CheckForNewLogs-ERROR-Web3-Null");
            }
        }

    }
}
