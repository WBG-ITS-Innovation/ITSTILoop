﻿using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using ITSTILoopLibrary.DTO;

namespace ITSTILoop.Context.Repositories
{
    public class PartyRepository : GenericRepository<Party>, IPartyRepository
    {
        public PartyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Party? GetPartyFromTypeAndId(PartyIdTypes partyIdentifierType, string partyIdentifier)
        {
            return _context.Parties.FirstOrDefault(k => k.PartyIdentifierType == partyIdentifierType && k.PartyIdentifier == partyIdentifier);            
        }
    }
}
