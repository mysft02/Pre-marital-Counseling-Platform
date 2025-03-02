using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.DTO;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface ITherapistService
    {
        Task<IActionResult> HandleGetAllTherapists();
        Task<IActionResult> HandleGetTherapistById(Guid id);
        Task<IActionResult> HandleUpdateTherapist(TherapistUpdateDTO therapistUpdateDTO, string? userId);
    }

    public class TherapistService : ControllerBase, ITherapistService
    {
        private readonly PmcsDbContext _context;

        public TherapistService(PmcsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetAllTherapists()
        {
            try
            {
                var therapists = _context.Therapists
                    .Include(c => c.Schedules)
                    .Include(c => c.Specialty).ThenInclude(m => m.Specification)
                    .Select(x => new TherapistDTO
                    {
                        TherapistId = x.TherapistId,
                        TherapistName = x.TherapistName,
                        Avatar = x.Avatar,
                        Status = x.Status,
                        ConsultationFee = x.ConsultationFee,
                        Description = x.Description,
                        MeetUrl = x.MeetUrl,
                        Schedules = x.Schedules.Select(s => new ScheduleResponseDTO
                        {
                            Date = s.Date,
                            Slot = s.Slot,
                            IsAvailable = s.IsAvailable,
                        }).ToList(),
                        Specifications = x.Specialty.Select(n => new SpecificationResponseDTO
                        {
                            Name = n.Specification.Name,
                            Description = n.Specification.Description,
                            Level = n.Specification.Level
                        }).ToList()
                    })
                    .ToList();

                return Ok(therapists);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetTherapistById(Guid id)
        {
            try
            {
                var therapist = _context.Therapists
                    .Include(c => c.Schedules)
                    .Include(c => c.Specialty).ThenInclude(m => m.Specification)
                    .Select(x => new TherapistDTO
                    {
                        TherapistId = x.TherapistId,
                        TherapistName = x.TherapistName,
                        Avatar = x.Avatar,
                        Status = x.Status,
                        ConsultationFee = x.ConsultationFee,
                        Description = x.Description,
                        MeetUrl = x.MeetUrl,
                        Schedules = x.Schedules.Select(s => new ScheduleResponseDTO
                        {
                            Date = s.Date,
                            Slot = s.Slot,
                            IsAvailable = s.IsAvailable,
                        }).ToList(),
                        Specifications = x.Specialty.Select(n => new SpecificationResponseDTO
                        {
                            Name = n.Specification.Name,
                            Description = n.Specification.Description,
                            Level = n.Specification.Level
                        }).ToList()
                    })
                    .FirstOrDefault(x => x.TherapistId == id);

                return Ok(therapist);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateTherapist(TherapistUpdateDTO therapistUpdateDTO, string? userId)
        {
            try
            {
                var therapist = _context.Therapists
                    .Include(c => c.Schedules)
                    .Include(c => c.Specialty)
                    .FirstOrDefault(x => x.TherapistId == therapistUpdateDTO.TherapistId);

                therapist.UpdatedBy = Guid.Parse(userId);
                therapist.UpdatedAt = DateTime.Now;
                therapist.Avatar = therapistUpdateDTO.Avatar;
                therapist.ConsultationFee = therapistUpdateDTO.ConsultationFee;
                therapist.Status = therapistUpdateDTO.Status;
                therapist.Description = therapistUpdateDTO.Description;
                therapist.MeetUrl = therapistUpdateDTO.MeetUrl;
                therapist.TherapistName = therapistUpdateDTO.TherapistName;

                _context.Therapists.Update(therapist);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(therapist);
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
