using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using FerryData.ActiveObjectsClassLibrary;

namespace FerryData.Engine.Models
{
    public class Workflow : Scenario
    {
        
        public Guid Uid { get; set; }

        public IWorkflowSettings Settings { get; set; }

        
        //public bool Finished { get; set; }

        public void AddStep(IWorkflowStep step)
        {

        }
    }
}
