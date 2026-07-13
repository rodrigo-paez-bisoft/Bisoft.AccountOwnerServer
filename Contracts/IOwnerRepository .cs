using Entities.models;
using Entities.Models;

namespace Contracts
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        //IEnumerable<Owner> GetOwners(OwnerParameters ownerParameters);  
        public PagedList<Owner> GetOwners(OwnerParameters ownerParameters);
        //IEnumerable<Owner> GetAllOwners();
        Owner GetOwnerById(Guid ownerId);
        Owner GetOwnerWithDetails(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);

    }


}