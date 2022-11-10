using ITSTILoopDTOLibrary;
using ITSTILoopSampleFSP.Models;

namespace ITSTILoopLibrary.SampleFSPServices
{
    public class AccountService
    {        
        public List<FspAccount> Accounts { get; set; } = new List<FspAccount>();
        public Dictionary<Guid, TransferRequestResponseDTO> TransferRequests = new Dictionary<Guid, TransferRequestResponseDTO>();

        public AccountService()
        {            
        }
    }
}
