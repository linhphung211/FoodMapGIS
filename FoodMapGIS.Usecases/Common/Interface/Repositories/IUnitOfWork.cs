using FoodMapGIS.Domain.Entities;

namespace FoodMapGIS.Application.Common.Interface.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> Users { get; }
        ICategoryRepository Categories { get; }
        IFoodPlaceRepository FoodPlaces { get; }
        IGenericRepository<MenuItem> MenuItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}