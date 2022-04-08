﻿using ITSTILoopDTOLibrary;
using ITSTILoopSampleFSP.Models;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopSampleFSP.Controllers
{

    [Route("itstiloop/[controller]")]
    [ApiController]
    public class ITSTILoopTransferController : ControllerBase
    {       
        private readonly ILogger<ITSTILoopTransferController> _logger;
        private readonly AccountService _accountService;

        public ITSTILoopTransferController(ILogger<ITSTILoopTransferController> logger, AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferRequestCompleteDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(Name = "confirmTransfer")]
        public ActionResult<TransferRequestCompleteDTO> Post(TransferRequestResponseDTO transferRequestResponseDTO)
        {
            var partyIdentifierDTO = transferRequestResponseDTO.To.PartyIdentifier;
            var account = _accountService.Accounts.FirstOrDefault(k => k.PartyDefinition.PartyIdentifier.Identifier == partyIdentifierDTO.Identifier && k.PartyDefinition.PartyIdentifier.PartyIdentifierType == partyIdentifierDTO.PartyIdentifierType);
            if (account != null)
            {
                account.TransferIn(transferRequestResponseDTO.Amount);
                account.Transactions.Add(new FspTransaction() { Amount = transferRequestResponseDTO.Amount, Timestamp = DateTime.Now, TransactionType = TransactionTypes.MoneyRecieved });
                return new TransferRequestCompleteDTO() { TransferId = transferRequestResponseDTO.TransferId, Fullfilment = "Fullfilled" };
            }
            return NotFound();
        }
    }
}