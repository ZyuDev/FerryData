using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.Client.Shared
{
    public partial class WorkflowActionStepSettingsEdit: ComponentBase
    {
        [Parameter]
        public WorkflowActionStepSettings Step { get; set; }
    }
}
