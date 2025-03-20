using Kventin.Services.Dtos.Schedule;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kventin.WebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "SuperUser, AdminSchedule")]
    [Route("api/schedule")]
    public class ScheduleController(IScheduleService scheduleService) : ControllerBase
    {
        private readonly IScheduleService _scheduleService = scheduleService;

        /// <summary>
        /// Получить расписание на учебный год
        /// </summary>
        /// <param name="dto">Передается ScheduleDto</param>
        /// <returns>Возвращает ReturnScheduleDto - элементы (занятия) расписания</returns>
        [HttpPost]
        public async Task<ActionResult<ReturnScheduleDto>> GetSchedule(ScheduleDto dto)
        {
            try
            {
                return Ok(await _scheduleService.GetSchedule(dto));
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Добавить элемент расписания
        /// </summary>
        /// <param name="dto">AddScheduleItemDto</param>
        /// <returns></returns>
        [HttpPost("addItem")]
        public async Task<ActionResult> AddScheduleItem(AddScheduleItemDto dto)
        {
            try
            {
                await _scheduleService.AddScheduleItem(dto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Редактировать элемент расписания
        /// </summary>
        /// <param name="itemId">Id элемента расписания</param>
        /// <param name="dto">UpdateScheduleItemDto</param>
        /// <returns></returns>
        [HttpPost("updateItem/{itemId}")]
        public async Task<ActionResult> EditScheduleItem(int itemId, UpdateScheduleItemDto dto)
        {
            try
            {
                await _scheduleService.UpdateScheduleItem(dto, itemId);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Удалить элемент расписания
        /// </summary>
        /// <param name="itemId">Id элемента расписания</param>
        /// <returns></returns>
        [HttpDelete("deleteItem/{itemId}")]
        public async Task<ActionResult> DeleteScheduleItem(int itemId)
        {
            await _scheduleService.DeleteScheduleItem(itemId);
            
            return Ok();
        }

        /// <summary>
        /// Создать сущность расписания
        /// </summary>
        /// <param name="dto">ScheduleDto</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult> CreateSchedule(ScheduleDto dto)
        {
            try
            {
                await _scheduleService.CreateSchedule(dto);
                return Ok();
            }
            catch (EntityAlreadyCreatedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
