using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO.Quiz;
using SWP391.DTO.Schedule;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IScheduleService
    {
        Task<IActionResult> HandleCreateSchedule(ScheduleCreateDTO scheduleCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllSchedules();
        Task<IActionResult> HandleGetScheduleById(Guid id);
        Task<IActionResult> HandleGetScheduleByTherapistId(Guid id);
        Task<IActionResult> HandleUpdateSchedule(ScheduleUpdateDTO scheduleUpdateDTO, string? userId);
    }

    public class ScheduleService : ControllerBase, IScheduleService
    {
        private readonly PmcsDbContext _context;

        public ScheduleService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleCreateSchedule(ScheduleCreateDTO scheduleCreateDTO, string? userId)
        {
            try
            {
                var schedule = new Schedule
                {
                    TherapistId = scheduleCreateDTO.TherapistId,
                    Date = scheduleCreateDTO.Date,
                    Slot = scheduleCreateDTO.Slot,
                    IsAvailable = scheduleCreateDTO.IsAvailable,
                };

                var check = true;
                while (check)
                {
                    var id = Guid.NewGuid();
                    var checkId = _context.Schedules.FirstOrDefault(x => x.ScheduleId == id);
                    if (checkId == null)
                    {
                        schedule.ScheduleId = id;
                        check = false;
                    }
                }

                _context.Schedules.Add(schedule);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(schedule);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetAllSchedules()
        {
            try
            {
                var schedules = _context.Schedules
                    .ToList();

                return Ok(schedules);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetScheduleById(Guid id)
        {
            try
            {
                var schedule = _context.Schedules
                    .Where(x => x.ScheduleId == id);

                return Ok(schedule);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetScheduleByTherapistId(Guid id)
        {
            try
            {
                var schedule = _context.Schedules
                    .Include(c => c.Bookings)
                    .Where(x => x.TherapistId == id);

                return Ok(schedule);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public Task<IActionResult> HandleUpdateSchedule(ScheduleUpdateDTO scheduleUpdateDTO, string? userId)
        {
            throw new NotImplementedException();
        }
    }
}
