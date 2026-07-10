using Entities.models;
using Entities.Models;

namespace Contracts
{
    public interface IOwnerRepository
    {
        IEnumerable<Owner> GetAllOwners();
        Owner GetOwnerById(Guid ownerId);
    }
}