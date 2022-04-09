using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using ITSTILoopDTOLibrary;
using Microsoft.EntityFrameworkCore;


namespace ITSTILoop.Context.Repositories
{
    public enum TransferResults { Success, SourceNotFound, DestinationNotFound, SourceAccountNotFound, DestinationAccountNotFound, InsufficientFunds };
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public TransferResults MakeTransfer(int from, int to, decimal amount, CurrencyCodes currency, Guid traceId)
        {
            TransferResults result = TransferResults.Success;
            var sourceParticipant = _context.Participants.Include(k => k.Accounts).FirstOrDefault(k => k.ParticipantId == from);
            var destinationParticipant = _context.Participants.Include(k => k.Accounts).FirstOrDefault(k => k.ParticipantId == to);
            if (sourceParticipant == null)
            {
                result = TransferResults.SourceNotFound;
                return result;
            }
            if (destinationParticipant == null)
            {
                result = TransferResults.DestinationNotFound;
                return result;
            }
            var sourceAccount = sourceParticipant.Accounts.FirstOrDefault(k => k.Currency == currency);
            var destinationAccount = destinationParticipant.Accounts.FirstOrDefault(k => k.Currency == currency);
            if (sourceAccount == null)
            {
                result = TransferResults.SourceAccountNotFound;
                return result;
            }
            if (destinationAccount == null)
            {
                result = TransferResults.DestinationAccountNotFound;
                return result;
            }
            sourceAccount.TransferOut(amount);
            destinationAccount.TransferIn(amount);
            Transaction transaction = new Transaction() { Amount = amount, Currency = currency, From = from, To = to, Timestamp = DateTime.Now, TraceId = traceId, TransactionType = TransactionTypes.Transfer };
            _context.Transactions.Add(transaction);
            Save();
            return result;
        }
    }
}
