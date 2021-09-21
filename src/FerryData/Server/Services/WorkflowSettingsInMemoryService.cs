using FerryData.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FerryData.Server.Services
{
    public class WorkflowSettingsInMemoryService : IWorkflowSettingsService
    {

        private List<WorkflowSettings> _items = new List<WorkflowSettings>();

        public WorkflowSettingsInMemoryService()
        {
            AddTestData();

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

        private void AddTestData()
        {

            var workflow1 = new WorkflowSettings() { Title = "Workflow with sleep", Memo = "Workflow with sleep step" };
            var sleepStep = new WorkflowActionStepSettings()
            {
                Title = "Sleep 1 second",
                Name = "sleep",
                Action = new WorkflowSleepAction() { DelayMilliseconds = 1000 }
            };
            workflow1.Steps.Add(sleepStep);

            _items.Add(workflow1);


            var workflow2 = new WorkflowSettings() { Title = "Workflow with HTTP", Memo = "Workflow with HTTP GET request" };
            var httpStep = new WorkflowActionStepSettings()
            {
                Title = "Get rates",
                Name = "get_rates",
                Action = new WorkflowHttpAction()
                {
                    Url = "http://api.exchangeratesapi.io/v1/latest?access_key=a5cf9da55cb835d0a633a7825b3aa8b5"
                }
            };
            workflow2.AddStep(httpStep);

            _items.Add(workflow2);
            _items.Add(new WorkflowSettings() { Title = "Workflow without steps", Memo = "Empty workflow" });

        }

    }
}
