using ITSTILoop.Model;
using ITSTILoop.Model.Interfaces;

namespace ITSTILoop.Context.Repositories
{
    public interface ISettlementWindowRepository : IGenericRepository<SettlementWindow>
    {
        void CloseOpenSettlementWindow();
        SettlementWindow CreateNewSettlementWindow();
        void SettleSettlementWindow();
        void UpdateSettlementWindow();
    }
}