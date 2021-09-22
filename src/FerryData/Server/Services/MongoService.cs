using FerryData.Engine.Abstract.Service;
using FerryData.Engine.Environment;
using FerryData.Engine.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FerryData.Server.Services
{
    public class MongoService<T> : IMongoService<T>
        where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoService(IMongoDatabaseSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString)
           .GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        private string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public async Task<int> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);

            return 1;
        }

        public async Task<int> DeleteAsync(Guid uid)
        {
            await _collection.DeleteOneAsync(x => x.Uid == uid);

            return 1;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(new BsonDocument())
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid uid)
        {
            return await _collection.Find(x => x.Uid == uid)
              .FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> uids)
        {
            return await _collection.Find(x => uids.Contains(x.Uid))
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate)
            .ToListAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(x => x.Uid == entity.Uid, entity);

            return 1;
        }

        public async Task<long> Count()
        {
            var count = await _collection.CountDocumentsAsync(new BsonDocument());

            return count;
        }
    }
}
