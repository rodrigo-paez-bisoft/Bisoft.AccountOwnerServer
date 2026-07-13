using Contracts;
using Entities;
using Entities.Models;
using Entities.models;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public Account GetAccountById(Guid Id)
        {
            return FindByCondition(account => account.Id.Equals(Id)).FirstOrDefault();
        }
        public PagedList<Account> GetAccountsByOwner(Guid ownerId, AccountParameters accountParameters)
        {
            var accounts = FindByCondition(a => a.OwnerId.Equals(ownerId)).OrderBy(a => a.Id);
            return PagedList<Account>.ToPagedList(accounts,
                accountParameters.PageNumber,
                accountParameters.PageSize);
        }
        public IEnumerable<Account> AccountsByOwner(Guid ownerId)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
        }
    }
}