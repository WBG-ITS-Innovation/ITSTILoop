using ITSTILoop.Model;

namespace ITSTILoop.Context.Repositories.Interfaces
{
    public interface ISettlementWindowRepository : IGenericRepository<SettlementWindow>
    {
        void CloseOpenSettlementWindow();
        SettlementWindow CreateNewSettlementWindow();
        void SettleSettlementWindow();
        void UpdateSettlementWindow();
    }
}