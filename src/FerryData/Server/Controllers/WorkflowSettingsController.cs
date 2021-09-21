//using FerryData.Engine.JsonConverters;
using FerryData.Engine.Models;
using FerryData.Server.Services;
using FerryData.Shared.Helpers;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FerryData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowSettingsController : ControllerBase
    {
        private readonly IWorkflowSettingsServiceAsync _dbService;

        //private JsonSerializerOptions options = new JsonSerializerOptions
        //{
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    Converters = { new IWorkflowStepSettingsConverter(), new IWorkflowStepActionConverter() }
        //};

        public WorkflowSettingsController(IWorkflowSettingsServiceAsync db)
        {
            _dbService = db;
        }

        [HttpGet("GetCollection")]
        public async Task<IEnumerable<WorkflowSettings>> GetCollection()
        {
            return await _dbService.GetCollection();
        }

        [HttpGet("GetItem/{guid}")]
        public async Task<IActionResult> GetItem(Guid guid)
        {
            var responseDto = new ResponseDto<WorkflowSettings>();

            var item = await _dbService.GetItem(guid);

            if (item == null)
            {
                responseDto.Status = -1;
                responseDto.Message = "Item not found";
            }
            else
            {
                responseDto.Data = item;
            }

            // Версия System.Text.Json
            //var json = JsonSerializer.Serialize(item, options);

            var json = JsonConvert.SerializeObject(item, Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            return Ok(json);
        }


        [HttpPost("UpdateItem")]
        public async Task<ResponseDto<int>> UpdateItem()
        {
            var responseDto = new ResponseDto<int>();

            // Parse manual because standard parser cannot parse steps.
            string requestBody = "";
            using (var reader = new StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
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
                responseDto.Data = await _dbService.Update(item);
            }

            return responseDto;
        }

        [HttpDelete("RemoveItem/{guid}")]
        public async Task<ResponseDto<int>> RemoveItem(Guid guid)
        {
            var responseDto = new ResponseDto<int>();

            responseDto.Data = await _dbService.Remove(guid);

            return responseDto;
        }

        [HttpPut("AddItem")]
        public async Task<ResponseDto<int>> AddItem()
        {
            var responseDto = new ResponseDto<int>();

            // Parse manual because standard parser cannot parse steps.
            string requestBody = "";
            using (var reader = new StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            WorkflowSettings item = null;
            try
            {

                item = JsonConvert.DeserializeObject<WorkflowSettings>(requestBody,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                // Версия System.Text.Json
                //item = System.Text.Json.JsonSerializer.Deserialize<WorkflowSettings>(requestBody, options);
            }
            catch (Exception e)
            {
                responseDto.Message = $"Parse error. Message {e.Message}";
                responseDto.Status = -1;
            }

            if (item != null)
            {
                responseDto.Data = await _dbService.Add(item);
            }

            return responseDto;
        }
    }
}
