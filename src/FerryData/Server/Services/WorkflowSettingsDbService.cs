using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FerryData.Engine.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FerryData.Server.Services
{
    public class WorkflowSettingsDbServiceAsync : IWorkflowSettingsServiceAsync
    {   
        private readonly IMongoCollection<WorkflowSettings> _workflowSettings;

        public WorkflowSettingsDbServiceAsync()
        {
            string connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString");
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            
            // TODO: вынести инициализацию коллекции либо в отдельное свойство, либо в EnvironmentVariable
            _workflowSettings = database.GetCollection<WorkflowSettings>("test");
        }

        public async Task<List<WorkflowSettings>> GetCollection()
        {
            var result = await _workflowSettings.Find(new BsonDocument()).ToListAsync();
            return result;
        }
        
        // получаем один документ по id
        public async Task<WorkflowSettings> GetItem(Guid guid)
        {
            return await _workflowSettings.Find(w => w.Uid == guid).FirstOrDefaultAsync();
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
            await _workflowSettings.ReplaceOneAsync(w => w.Uid == workflowSettings.Uid, workflowSettings);
            return 1;
        }
        // удаление документа
        public async Task<int> Remove(Guid guid)
        {
            await _workflowSettings.DeleteOneAsync(p => p.Uid == guid);
            return 1;
        }
    }
}