using Contracts;
using Entities;
using Entities.models;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //Trae todos los owners y los ordena por nombre
        /*public IEnumerable<Owner> GetAllOwners()
        {
            return FindAll()
                .OrderBy(ow => ow.Name)
                .ToList();
        }*/
        

        public PagedList<Owner> GetOwners(OwnerParameters ownerParameters)
        {
            return PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }
        //Trae todos los owners y los ordena por nombre
        /*public IEnumerable<Owner> GetOwners(OwnerParameters ownerParameters)
        {
            return FindAll()
                .OrderBy(on => on.Name)
                .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                .Take(ownerParameters.PageSize)
                .ToList();
        }*/

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id==ownerId)
                    .FirstOrDefault();
        }
        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id == ownerId)
                .Include(ac => ac.Accounts)
                .FirstOrDefault();
        }
        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }
        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }
        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }
    }
}