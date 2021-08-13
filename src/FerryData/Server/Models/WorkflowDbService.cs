using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FerryData.Engine;
using FerryData.Engine.Models;

namespace FerryData.Server.Models
{
    public class WorkflowDbService
    {
        private IMongoCollection<Workflow> Workflows;

        public WorkflowDbService()
        {
            string connectionString = "mongodb://localhost:27017/ferryDB";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Workflows = database.GetCollection<Workflow>("workflow");
        }
        
        // получаем один документ по id
        public async Task<Workflow> GetWorkflow(string uid)
        {
            return await Workflows.Find(new BsonDocument("_id", new ObjectId(uid))).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task Create(Workflow workflow)
        {
            await Workflows.InsertOneAsync(workflow);
        }
        // обновление документа
        public async Task Update(Workflow workflow)
        {
            await Workflows.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(workflow.Uid.ToString())), workflow);
        }
        // удаление документа
        public async Task Remove(string uid)
        {
            await Workflows.DeleteOneAsync(new BsonDocument("_id", new ObjectId(uid)));
        }
    }
}