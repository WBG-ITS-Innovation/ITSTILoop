using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        TransferResults MakeTransfer(int from, int to, decimal amount, CurrencyCodes currency, Guid traceId);
    }
}