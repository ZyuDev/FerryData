﻿using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using FerryData.Engine.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    
    public abstract class WorkflowStepSettingsBase : IWorkflowStepSettings
    {
        [JsonIgnore]
        protected List<Guid> _inSteps = new List<Guid>();
        [JsonIgnore]
        protected List<Guid> _outSteps = new List<Guid>();

        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public WorkflowStepKinds Kind { get; set; }

        [JsonIgnore]
        public IEnumerable<Guid> InSteps => _inSteps;

        [JsonIgnore]
        public IEnumerable<Guid> OutSteps => _outSteps;

        [JsonIgnore]
        public virtual Guid? InUid
        {
            get
            {
                return _inSteps.FirstOrDefault();
            }
            set
            {

                if (_inSteps.Any())
                {
                    _inSteps.Clear();
                }

                if (value.HasValue)
                {
                    _inSteps.Add(value.Value);
                }

            }
        }

        [JsonIgnore]
        public virtual Guid? OutUid
        {
            get
            {
                return _outSteps.FirstOrDefault();
            }
            set
            {

                if (_outSteps.Any())
                {
                    _outSteps.Clear();
                }

                if (value.HasValue)
                {
                    _outSteps.Add(value.Value);
                }

            }
        }

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
