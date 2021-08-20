using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FerryData.Engine.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FerryData.Server.Services
{
    public class WorkflowSettingsDbService
    {   
        private readonly IMongoCollection<WorkflowSettings> _workflowSettings;

        public WorkflowSettingsDbService()
        {
            string connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString");
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            
            _workflowSettings = database.GetCollection<WorkflowSettings>("test");
        }

        public async Task<List<WorkflowSettings>> GetCollection()
        {
            var result = await _workflowSettings.Find(new BsonDocument()).ToListAsync();
            return result;
        }
        
        // получаем один документ по id
        public async Task<WorkflowSettings> GetItem(Guid id)
        {
            return await _workflowSettings.Find(w => w.Uid == id).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task<int> Add(WorkflowSettings workflowSettings)
        {
            await _workflowSettings.InsertOneAsync(workflowSettings);
            return 1;
        }
        // обновление документа
        public async Task<int> Update(WorkflowSettings workflowSettings)
        {
            await _workflowSettings.ReplaceOneAsync(new BsonDocument("_id", workflowSettings.Uid), workflowSettings);
            return 1;
        }
        // удаление документа
        public async Task<int> Remove(Guid id)
        {
            // var filter = Builders<WorkflowSettings>.Filter.Eq(key, value);
            var result = await _workflowSettings.DeleteOneAsync(p => p.Uid == id);
            // await _workflowSettings.DeleteOneAsync(_workflowSettings => _workflowSettings._id == value);
            return 1;
        }
    }
}