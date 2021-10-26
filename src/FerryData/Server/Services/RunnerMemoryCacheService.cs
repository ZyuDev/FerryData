using FerryData.Engine.Abstract.Service;
using FerryData.Engine.Runner;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.Server.Services
{
    public class RunnerMemoryCacheService: IRunnerMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _runId;

        public string ResultKey => $"runner_result_{_runId}";
        public string StopKey => $"runner_stop_{_runId}";


        public RunnerMemoryCacheService(IMemoryCache memoryCache, string runId)
        {
            _memoryCache = memoryCache;
            _runId = runId;

        }

        public void SetCache(string key, object value, int days, int hours, int minutes, int seconds)
        {

            var entry = _memoryCache.CreateEntry(key);
            entry.Value = value;
            entry.AbsoluteExpiration = DateTimeOffset.MaxValue;
            entry.SlidingExpiration = new TimeSpan(days, hours, minutes, seconds, 0);
            entry.Priority = CacheItemPriority.Normal;

            _memoryCache.Set(key, entry);

        }

        public void SetResult(WorkflowExecuteResultDto resultDto)
        {
            SetCache(ResultKey, resultDto, 0, 1, 0, 0);
        }

        public WorkflowExecuteResultDto GetResult()
        {
            var objectFromCache = GetCache(ResultKey);

            if (objectFromCache is WorkflowExecuteResultDto resultDto)
            {
                return resultDto;
            }
            else
            {
                return null;
            }
        }

        public void SetTaskStop()
        {
            SetCache(StopKey, true, 0, 1, 0, 0);
        }

        public bool IsStopped()
        {
            var cacheValue = GetCache(StopKey);

            var isStopped = cacheValue != null;

            return isStopped;

        }

        public void DropCache(string key)
        {
            _memoryCache.Remove(key);
        }

        public object GetCache(string key)
        {
            object result = null;

            if (_memoryCache.TryGetValue(key, out ICacheEntry entry))
            {
                result = entry?.Value;
            }

            return result;
        }
    }
}
