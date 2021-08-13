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
            string connectionString = "mongodb://localhost:27017/test";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            
            _workflowSettings = database.GetCollection<WorkflowSettings>("test");
        }
        
        // получаем один документ по id
        public async Task<WorkflowSettings> GetWorkflowSettings(string id)
        {
            return await _workflowSettings.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task Create(WorkflowSettings workflowSettings)
        {
            await _workflowSettings.InsertOneAsync(workflowSettings);
        }
        // обновление документа
        public async Task Update(WorkflowSettings workflowSettings)
        {
            await _workflowSettings.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(workflowSettings.Uid.ToString())), workflowSettings);
        }
        // удаление документа
        public async Task Remove(string id)
        {
            await _workflowSettings.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
    }
}