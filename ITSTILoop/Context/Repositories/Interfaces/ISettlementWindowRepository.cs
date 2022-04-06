using ITSTILoop.Model;

namespace ITSTILoop.Context.Repositories
{
    public interface ISettlementWindowRepository
    {
        void CloseOpenSettlementWindow();
        SettlementWindow CreateNewSettlementWindow();
        void SettleSettlementWindow();
        void UpdateSettlementWindow();
    }
}