using ITSTILoop.Model;

namespace ITSTILoop.Context.Repositories.Interfaces
{
    public interface ISettlementWindowRepository : IGenericRepository<SettlementWindow>
    {
        void CloseOpenSettlementWindow();
        SettlementWindow CreateNewSettlementWindow();        
        Dictionary<string, decimal> GetNetSettlementDictionary(int id);
        void SettleSettlementWindow();
        void UpdateSettlementWindow();
    }
}