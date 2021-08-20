using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FerryData.Engine.Models;

namespace FerryData.Server.Services
{
    public interface IWorkflowSettingsServiceAsync
    {
        Task<List<WorkflowSettings>> GetCollection();
        Task<WorkflowSettings>GetItem(Guid uid);
        Task<int> Add(WorkflowSettings item);
        Task<int> Update(WorkflowSettings item);
        Task<int> Remove(Guid uid);
    }
}