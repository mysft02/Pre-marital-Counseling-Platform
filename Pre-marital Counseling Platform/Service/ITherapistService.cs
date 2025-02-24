using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.DTO.Quiz;
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
                    .Include(c => c.Specialty)
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
                    .Include(c => c.Specialty)
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
