using FerryData.Engine.Abstract.Service;
using FerryData.Engine.Models;
using FerryData.Engine.Runner;
using FerryData.Server.Services;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FerryData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowRunnerController : ControllerBase
    {
        private readonly IMongoService<WorkflowSettings> _dbService;
        private readonly IMemoryCache _memoryCache;


        public WorkflowRunnerController(IMongoService<WorkflowSettings> dbService, IMemoryCache memoryCache)
        {
            _dbService = dbService;
            _memoryCache = memoryCache;
        }

        [HttpPost("Start")]
        public async Task<ResponseDto<int>> Start(WorkflowStartDto dto)
        {
            var result = new ResponseDto<int>();

            var item = await _dbService.GetByIdAsync(dto.SettingsUid);

            if (item == null)
            {
                result.Status = -1;
                result.Message = $"Settings not find {dto.SettingsUid}";
            }
            else
            {
                var cacheService = new RunnerMemoryCacheService(_memoryCache, dto.RunUid.ToString());

                Task.Run(() => StartWorkflow(item, cacheService)).ConfigureAwait(false);
            }

            return result;
        }

        [HttpGet("Check/{Uid}")]
        public WorkflowExecuteResultDto Check(Guid Uid)
        {

            var cacheService = new RunnerMemoryCacheService(_memoryCache, Uid.ToString());

            var executeResult = cacheService.GetResult();

            if (executeResult == null)
            {
                executeResult = new WorkflowExecuteResultDto();
            }

            return executeResult;
        }


        [HttpGet("Execute/{Uid}")]
        public async Task<WorkflowExecuteResultDto> Execute(Guid Uid)
        {
            var executeResult = new WorkflowExecuteResultDto();

            var item = await _dbService.GetByIdAsync(Uid);

            if (item == null)
            {
                executeResult.Status = -1;
                executeResult.Message = $"Settings not find {Uid}";
            }
            else
            {
                var runner = new WorkflowRunner(item, null);

                await runner.Run();
                executeResult = runner.PrepareExecuteResult();

               
            }

            return executeResult;
        }

        private void StartWorkflow(WorkflowSettings item, IRunnerMemoryCacheService cacheService)
        {
            var runner = new WorkflowRunner(item, cacheService);

            runner.Run().ConfigureAwait(false);
        }
    }
}
