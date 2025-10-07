using BattleGame.Shared.Database.Abstractions;

namespace BattleGame.Shared.Database.Repositories
{
    public class MongoRepository<T> : IBaseRepository<T> where T : IEntity
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(_filterBuilder.Eq(e => e.Id, entity.Id), entity);
        }

        public async Task DeleteAsync(T entity)
        {
            var filter = _filterBuilder.Eq(e => e.Id, entity.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            var filter = _filterBuilder.Where(expression);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            var filter = _filterBuilder.Where(expression);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _collection.Find(_filterBuilder.Eq(e => e.Id, id)).FirstOrDefaultAsync();
        }
    }
}
