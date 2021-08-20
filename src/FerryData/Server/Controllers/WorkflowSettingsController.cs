using FerryData.Engine.Models;
using FerryData.Server.Services;
using FerryData.Shared.Helpers;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkflowSettingsController : ControllerBase
    {
        private IWorkflowSettingsService _service;
        private readonly WorkflowSettingsDbService _dbService;

        public WorkflowSettingsController(IWorkflowSettingsService service, WorkflowSettingsDbService db)
        {
            _service = service;
            _dbService = db;
        }

        [HttpGet("GetCollection")]
        public async Task<IEnumerable<WorkflowSettings>> GetCollection()
        {
            return await _dbService.GetCollection();
        }

        [HttpGet("GetItem/{uid}")]
        public IActionResult GetItem(Guid uid)
        {
            var responseDto = new ResponseDto<WorkflowSettings>();
        
            // var item = _service.GetItem(uid);
            var item = _dbService.GetItem(uid).GetAwaiter().GetResult(); 
            
            if (item == null)
            {
                responseDto.Status = -1;
                responseDto.Message = "Item not found";
            }
            else
            {
                responseDto.Data = item;
            }
        
            var json = JsonHelper.Serialize(item);
        
            return Ok(json);
        }
        
        
        [HttpPost("UpdateItem")]
        public ResponseDto<int> UpdateItem()
        {
            var responseDto = new ResponseDto<int>();
        
            // Parse manual because standard parser cannot parse steps.
            string requestBody = "";
            using (var reader = new StreamReader(Request.Body))
            {
                requestBody = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }
        
            WorkflowSettings item = null;
            try
            {
                var parser = new WorkflowSettingsParser();
                item = parser.Parse(requestBody);
        
            }
            catch (Exception e)
            {
                responseDto.Message = $"Parse error. Message {e.Message}";
                responseDto.Status = -1;
            }
        
            if (item != null)
            {
                // responseDto.Data = _service.Update(item);
                responseDto.Data = _dbService.Update(item).GetAwaiter().GetResult();
            }
        
            return responseDto;
        }
        
        [HttpDelete("RemoveItem/{id}")]
        // public ResponseDto<int> RemoveItem(WorkflowSettings item)
        public ResponseDto<int> RemoveItem(Guid id)
        {
            var responseDto = new ResponseDto<int>();
        
            // responseDto.Data = _service.Remove(item.Uid);
            responseDto.Data = _dbService.Remove(id).GetAwaiter().GetResult();
        
            return responseDto;
        }

        [HttpPut("AddItem")]
        public ResponseDto<int> AddItem(WorkflowSettings item)
        {
            var responseDto = new ResponseDto<int>();
            
            responseDto.Data = _dbService.Add(item).GetAwaiter().GetResult();
        
            return responseDto; 
        }
    }
}
