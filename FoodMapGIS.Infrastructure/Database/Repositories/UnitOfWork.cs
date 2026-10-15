using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Domain.Entities;
using FoodMapGIS.Infrastructure.Database.Persistence;

namespace FoodMapGIS.Infrastructure.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<User> Users { get; }
        public ICategoryRepository Categories { get; }
        public IFoodPlaceRepository FoodPlaces { get; }
        public IGenericRepository<MenuItem> MenuItems { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new GenericRepository<User>(_context);
            Categories = new CategoryRepository(_context);
            FoodPlaces = new FoodPlaceRepository(_context);
            MenuItems = new GenericRepository<MenuItem>(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}