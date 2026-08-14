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
    public class WorkshopController : BaseApiController
    {
        public WorkshopController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,Staff")]
        public async Task<ActionResult<List<WorkshopsDTO>>> GetWorkshopsTemplate()
        {
            var result = await Mediator.Send(new GetWorkshopTemplateQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddWorkshop([FromBody] AddWorkshopCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Thêm xưởng thành công.");
        }

        [HttpPut("insert")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertWorkshopAsync([FromBody] InsertWorkshopCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Chèn xưởng thành công.");
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateWorkshopCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Cập nhật thành công");
        }

        [HttpPut("swap-workshop")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwapAsync([FromBody] SwapWorkshopCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Đổi 2 xưởng thành công.");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteWorkshopCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Xóa xưởng thành công");
        }
    }
}
