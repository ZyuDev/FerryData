using FerryData.Engine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FerryData.Server.Services
{
    public interface IWorkflowSettingsServiceAsync
    {
        Task<List<WorkflowSettings>> GetCollection();
        Task<WorkflowSettings> GetItem(Guid uid);
        Task<int> Add(WorkflowSettings item);
        Task<int> Update(WorkflowSettings item);
        Task<int> Remove(Guid uid);
    }
}