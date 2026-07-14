using Contracts;
using Entities;
using Entities.Helpers;
using Entities.models;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        private ISortHelper<Owner> _sortHelper;
        public OwnerRepository(RepositoryContext repositoryContext, ISortHelper<Owner> sortHelper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
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
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
                                o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth);

            SearchByName(ref owners, ownerParameters.Name);
            //ApplySort(ref owners, ownerParameters.OrderBy);
            var sortedOwners = _sortHelper.ApplySort(owners, ownerParameters.OrderBy);

            return PagedList<Owner>.ToPagedList(owners.OrderBy(on => on.Name),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }
        private void SearchByName(ref IQueryable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;
            owners = owners.Where(o => o.Name.ToLower().Contains(ownerName.Trim().ToLower()));
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

        //Metodo para aplicar el ordenamiento dinámico a la lista de owners según el parámetro orderByQueryString
        private void ApplySort(ref IQueryable<Owner> owners, string orderByQueryString)
        {
            if (!owners.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                owners = owners.OrderBy(x => x.Name);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Owner).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                owners = owners.OrderBy(x => x.Name);
                return;
            }
            owners = owners.OrderBy(orderQuery);
        }
    }
}