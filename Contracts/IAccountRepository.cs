using Entities.models;
using Entities.Models;

namespace Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        IEnumerable<Account> AccountsByOwner(Guid ownerId);
        public PagedList<Account> GetAccountsByOwner(Guid ownerId, AccountParameters accountParameters);
        Account GetAccountById(Guid accountId);
    }

}