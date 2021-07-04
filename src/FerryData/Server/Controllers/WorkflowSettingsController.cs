using FerryData.Engine.Models;
using FerryData.Server.Services;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkflowSettingsController : ControllerBase
    {
        private IWorkflowSettingsService _service;

        public WorkflowSettingsController(IWorkflowSettingsService service)
        {
            _service = service;

        }

        [HttpGet("GetCollection")]
        public IEnumerable<WorkflowSettings> GetCollection()
        {
            var collection = _service.GetCollection();

            return collection;
        }

        [HttpGet("GetItem/{uid}")]
        public ResponseDto<WorkflowSettings> GetItem(Guid uid)
        {
            var responseDto = new ResponseDto<WorkflowSettings>();

            var item = _service.GetItem(uid);

            if (item == null)
            {
                responseDto.Status = -1;
                responseDto.Message = "Item not found";
            }
            else
            {
                responseDto.Data = item;
            }

            return responseDto;
        }

        [HttpPost("UpdateItem")]
        public ResponseDto<int> UpdateItem(WorkflowSettings item)
        {
            var responseDto = new ResponseDto<int>();

            responseDto.Data = _service.Update(item);

            return responseDto;
        }

        [HttpPost("RemoveItem")]
        public ResponseDto<int> RemoveItem(WorkflowSettings item)
        {
            var responseDto = new ResponseDto<int>();

            responseDto.Data = _service.Remove(item.Uid);

            return responseDto;
        }
    }
}
