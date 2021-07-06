using FerryData.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.Server.Services
{
    public class WorkflowSettingsInMemoryService : IWorkflowSettingsService
    {

        private List<WorkflowSettings> _items = new List<WorkflowSettings>();

        public WorkflowSettingsInMemoryService()
        {
            // Add test data
            _items.Add(new WorkflowSettings() { Title = "Workflow 1", Memo = "Test workflow 1!" });
            _items.Add(new WorkflowSettings() { Title = "Workflow 2", Memo = "Test workflow 2!" });
            _items.Add(new WorkflowSettings() { Title = "Workflow 3", Memo = "Test workflow 3!" });

        }

        public List<WorkflowSettings> GetCollection()
        {
            return _items;
        }

        public WorkflowSettings GetItem(Guid uid)
        {
            return _items.FirstOrDefault(x => x.Uid == uid);
        }

        public int Add(WorkflowSettings item)
        {
            if (item == null)
            {
                return 0;
            }

            if (_items.Any(x => x.Uid == item.Uid))
            {
                return 0;
            }
            else
            {
                _items.Add(item);

                return 1;
            }
        }

        public int Update(WorkflowSettings item)
        {
            if (item == null)
            {
                return 0;
            }

            var savedItem = _items.FirstOrDefault(x => x.Uid == item.Uid);
            if (savedItem == null)
            {
                _items.Add(item);
                return 1;
            }
            else
            {
                var ind = _items.IndexOf(savedItem);
                _items[ind] = item;

                return 1;
            }
        }

        public int Remove(Guid uid)
        {
            var savedItem = _items.FirstOrDefault(x => x.Uid == uid);

            if (savedItem == null)
            {
                return 0;
            }
            else
            {
                _items.Remove(savedItem);
                return 1;
            }

        }


    }
}
