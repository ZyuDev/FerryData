using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FerryData.Engine.Models
{
    public class WorkflowSettings : IWorkflowSettings
    {
       // private List<IWorkflowStepSettings> _steps = new List<IWorkflowStepSettings>();
       
       [BsonId] 
       public ObjectId _id { get; set; } = ObjectId.GenerateNewId();
       
        public Guid Uid { get; set; } = Guid.NewGuid();
        
        public string Title { get; set; }
        
        public string Memo { get; set; }
        
        public List<IWorkflowStepSettings> Steps { get; set; } = new List<IWorkflowStepSettings>();

        public void AddStep(IWorkflowStepSettings step)
        {
            Steps.Add(step);
        }

        public void ClearSteps()
        {
            Steps.Clear();
        }

        public void RemoveStep(IWorkflowStepSettings step)
        {
            Steps.Remove(step);
        }


    }
}
