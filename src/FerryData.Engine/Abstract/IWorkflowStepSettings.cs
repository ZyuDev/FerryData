﻿using FerryData.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FerryData.Engine.JsonConverters;
using System.Text.Json.Serialization;

namespace FerryData.Engine.Abstract
{
    [JsonInterfaceConverter(typeof(IWorkflowStepSettingsConverter))]
    public interface IWorkflowStepSettings
    {
        Guid Uid { get; set; }
        string Title { get; set; }
        string Name { get; set; }
        string Memo { get; set; }
        WorkflowStepKinds Kind { get; set; }

        [JsonIgnore]
        IEnumerable<Guid> InSteps { get; }
        [JsonIgnore]
        IEnumerable<Guid> OutSteps { get; }

        Guid? InUid { get; set; }
        Guid? OutUid { get; set; }

    }
}
