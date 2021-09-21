using FerryData.Engine.Models;
using System;
using System.Collections.Generic;

namespace FerryData.Server.Services
{
    public interface IWorkflowSettingsService
    {
        List<WorkflowSettings> GetCollection();
        WorkflowSettings GetItem(Guid uid);
        int Add(WorkflowSettings item);
        int Update(WorkflowSettings item);
        int Remove(Guid uid);

    }
}
