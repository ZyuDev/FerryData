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
            return await _workflowSettings.Find(new BsonDocument()).ToListAsync();
        }
        
        // получаем один документ по id
        public async Task<WorkflowSettings> GetItem(string key, string value)
        {
            return await _workflowSettings.Find(new BsonDocument(key, value)).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task Create(WorkflowSettings workflowSettings)
        {
            await _workflowSettings.InsertOneAsync(workflowSettings);
        }
        // обновление документа
        public async Task<int> Update(WorkflowSettings workflowSettings)
        {
            await _workflowSettings.ReplaceOneAsync(new BsonDocument("_id", workflowSettings._id), workflowSettings);
            return 1;
        }
        // удаление документа
        public async Task<int> Remove(string id)
        {
            // var filter = Builders<WorkflowSettings>.Filter.Eq(key, value);
            var result = await _workflowSettings.DeleteOneAsync(p => p._id == new ObjectId(id));
            // await _workflowSettings.DeleteOneAsync(_workflowSettings => _workflowSettings._id == value);
            return 1;
        }
    }
}