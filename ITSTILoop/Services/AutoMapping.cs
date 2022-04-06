﻿using AutoMapper;
using ITSTILoop.DTO;
using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Participant, ParticipantDTO>();
            CreateMap<RegisterParticipantDTO, Participant>();
            CreateMap<Party, PartyDTO>();
            CreateMap<RegisterPartyDTO, Party>();
            CreateMap<TransferRequestDTO, TransferRequest>();
            CreateMap<TransferRequestDTO, TransferRequestResponseDTO>().ForMember(k => k.To, act => act.Ignore());
        }
    }
}
