using ITSTILoop.Model;
using ITSTILoop.Model.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        TransferResults MakeTransfer(int from, int to, decimal amount, CurrencyCodes currency, Guid traceId);
    }
}