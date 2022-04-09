using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using Microsoft.EntityFrameworkCore;

namespace ITSTILoop.Context.Repositories
{
    public class SettlementWindowRepository : GenericRepository<SettlementWindow>, ISettlementWindowRepository
    {
        public SettlementWindowRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void SettleSettlementWindow()
        {
            SettlementWindow? settlementWindow = _context.SettlementWindows.Include(k => k.SettlementAccounts).FirstOrDefault(k => k.Status == SettlementWindowStatuses.Open);
            if (settlementWindow != null)
            {
                foreach (var participant in _context.Participants)
                {
                    Account? account = participant.Accounts.FirstOrDefault();
                    if (account != null)
                    {
                        SettlementAccount? settlementAccount = settlementWindow.SettlementAccounts.FirstOrDefault(k => k.AccountId == account.AccountId);
                        if (settlementAccount != null)
                        {
                            account.SettleIn(settlementAccount.NetSettlementAmount);
                        }
                    }
                }
                settlementWindow.Status = SettlementWindowStatuses.Settled;
                settlementWindow.ModifiedAt = DateTime.Now;
            }
            _context.SaveChanges();
        }

        public void CloseOpenSettlementWindow()
        {
            UpdateSettlementWindow();
            SettlementWindow? settlementWindow = _context.SettlementWindows.Include(k => k.SettlementAccounts).FirstOrDefault(k => k.Status == SettlementWindowStatuses.Open);
            if (settlementWindow != null)
            {
                settlementWindow.Status = SettlementWindowStatuses.Closed;
                settlementWindow.ModifiedAt = DateTime.Now.ToUniversalTime();
            }
            _context.SaveChanges();
        }

        public SettlementWindow CreateNewSettlementWindow()
        {
            SettlementWindow settlementWindow = new SettlementWindow();
            foreach (var participant in _context.Participants.Include(k => k.Accounts))
            {
                Account? account = participant.Accounts.FirstOrDefault();
                if (account != null)
                {
                    SettlementAccount settlementAccount = new SettlementAccount() { AccountId = account.AccountId, ParticipantName = participant.Name, NetSettlementAmount = account.Settlement };
                    settlementWindow.SettlementAccounts.Add(settlementAccount);
                }
            }
            _context.SettlementWindows.Add(settlementWindow);
            _context.SaveChanges();
            return settlementWindow;
        }

        public void UpdateSettlementWindow()
        {
            SettlementWindow? settlementWindow = _context.SettlementWindows.Include(k => k.SettlementAccounts).FirstOrDefault(k => k.Status == SettlementWindowStatuses.Open);
            if (settlementWindow != null)
            {
                foreach (var participant in _context.Participants.Include(k => k.Accounts))
                {
                    Account? account = participant.Accounts.FirstOrDefault();
                    if (account != null)
                    {
                        SettlementAccount? settlementAccount = settlementWindow.SettlementAccounts.FirstOrDefault(k => k.AccountId == account.AccountId);
                        if (settlementAccount == null)
                        {
                            settlementAccount = new SettlementAccount() { AccountId = account.AccountId, ParticipantName = participant.Name, NetSettlementAmount = account.Position };
                            settlementWindow.SettlementAccounts.Add(settlementAccount);
                            _context.SettlementWindows.Add(settlementWindow);
                        }
                        else
                        {
                            settlementAccount.NetSettlementAmount = account.Position;
                        }
                    }
                }
                settlementWindow.ModifiedAt = DateTime.Now;
            }
            _context.SaveChanges();
        }
    }
}
