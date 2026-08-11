using Application.DTOs.Response;
using Application.Features.Workshop.Command.AddWorkshop;
using Application.Features.Workshop.Command.DeleteWorkshop;
using Application.Features.Workshop.Command.InsertWorkshop;
using Application.Features.Workshop.Command.SwapWorkshop;
using Application.Features.Workshop.Command.UpdateWorkshop;
using Application.Features.Workshop.Queries.GetWorkshopTemplate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopController : ControllerBase
    {
        private readonly ISender _sender;
        public WorkshopController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,Staff")]
        public async Task<List<WorkshopsDTO>> GetWorkshopsTemplate()
        {
            return await _sender.Send(new GetWorkshopTemplateQuery());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddWorkshop([FromBody] AddWorkshopCommand command)
        {
            await _sender.Send(command);
            return Ok("Workshop added successfully");
        }

        [HttpPut("insert")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertWorkshopAsync([FromQuery] InsertWorkshopCommand command)
        {
            await _sender.Send(command);
            return Ok("Chèn xưởng thành công.");
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromQuery] UpdateWorkshopCommand command)
        {
            await _sender.Send(command);
            return Ok("Cập nhật thành công");
        }

        [HttpPut("swap-workshop")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwapAsync([FromQuery] SwapWorkshopCommand command)
        {
            await _sender.Send(command);
            return Ok("Đổi 2 xưởng thành công.");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromQuery] DeleteWorkshopCommand command)
        {
            await _sender.Send(command);
            return Ok("Xóa xưởng thành công");
        }
    }
}
