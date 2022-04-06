using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories
{
    public interface ITransactionRepository
    {
        TransferResults MakeTransfer(int from, int to, decimal amount, CurrencyCodes currency, Guid traceId);
    }
}