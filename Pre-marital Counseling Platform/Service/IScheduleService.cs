using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IScheduleService
    {
        Task<IActionResult> HandleCreateSchedule(List<ScheduleCreateDTO> scheduleCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllSchedules();
        Task<IActionResult> HandleGetScheduleById(Guid id);
        Task<IActionResult> HandleGetScheduleByTherapistId(Guid id);
        Task<IActionResult> HandleUpdateSchedule(List<ScheduleUpdateDTO> scheduleUpdateDTO, string? userId);
    }

    public class ScheduleService : ControllerBase, IScheduleService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public ScheduleService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleCreateSchedule(List<ScheduleCreateDTO> scheduleCreateDTO, string? userId)
        {
            try
            {
                var scheduleQuery = _context.Schedules.AsQueryable();
                foreach(var item in scheduleCreateDTO)
                {
                    var scheduleMapped = _mapper.Map<Schedule>(item);
                    var duplicate = scheduleQuery.FirstOrDefault(x => x.TherapistId == scheduleMapped.TherapistId
                                           && DateOnly.FromDateTime(x.Date) == DateOnly.FromDateTime(scheduleMapped.Date)
                                           && x.Slot == scheduleMapped.Slot);

                    if (duplicate == null)
                    {
                        _context.Schedules.Add(scheduleMapped);
                    }
                    else
                    {
                        duplicate.Status = scheduleMapped.Status;
                    }
                }

                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok("Create Successfully");
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

        public async Task<IActionResult> HandleUpdateSchedule(List<ScheduleUpdateDTO> scheduleUpdateDTO, string? userId)
        {
            try
            {
                var schedule = _context.Schedules.AsQueryable();

                foreach (var item in scheduleUpdateDTO)
                {
                    var scheduleMapped = _mapper.Map<Schedule>(item);
                    _context.Schedules.Update(scheduleMapped);
                }

                if(_context.SaveChanges() > 0)
                {
                    return Ok(schedule);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
