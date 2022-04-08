﻿using ITSTILoop.DTO;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services.Interfaces
{
    public interface IParticipantPartyQueryService
    {
        Task<PartyLookupServiceResult> LookupPartyAsync(PartyIdentifierDTO partyQuery, Uri endpoint);
    }
}