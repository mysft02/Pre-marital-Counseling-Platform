using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;
using SWP391.Migrations;

namespace SWP391.Service
{
    public interface ITherapistService
    {
        Task<IActionResult> HandleGetAllTherapists();
        Task<IActionResult> HandleGetTherapistById(Guid id);
        Task<IActionResult> HandleGetTherapistByName(string name);
        Task<IActionResult> HandleGetTherapistBySpecificationId(Guid id);
        Task<IActionResult> HandleUpdateTherapist(TherapistUpdateDTO therapistUpdateDTO, string? userId);
        Task<IActionResult> GetTherapistByRating(decimal rating);
    }

    public class TherapistService : ControllerBase, ITherapistService
    {
        private readonly PmcsDbContext _context;
        private readonly IBookingService _bookingService;

        public TherapistService(PmcsDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
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
                            ScheduleId = s.ScheduleId,
                            Date = s.Date,
                            Slot = s.Slot,
                            Status = s.Status,
                        }).ToList(),
                        Specifications = x.Specialty
                        .Select(n => new SpecificationResponseDTO
                        {
                            Name = n.Specification.Name,
                            Description = n.Specification.Description,
                            Level = n.Specification.Level,
                            Status = n.Status
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
                            ScheduleId = s.ScheduleId,
                            Date = s.Date,
                            Slot = s.Slot,
                            Status = s.Status,
                        }).ToList(),
                        Specifications = x.Specialty
                        .Where(m => m.Status == SpecificationStatusEnum.Active)
                        .Select(n => new SpecificationResponseDTO
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

        public async Task<IActionResult> HandleGetTherapistByName(string name)
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
                            ScheduleId = s.ScheduleId,
                            Date = s.Date,
                            Slot = s.Slot,
                            Status = s.Status,
                        }).ToList(),
                        Specifications = x.Specialty
                        .Where(m => m.Status == SpecificationStatusEnum.Active)
                        .Select(n => new SpecificationResponseDTO
                        {
                            Name = n.Specification.Name,
                            Description = n.Specification.Description,
                            Level = n.Specification.Level
                        }).ToList()
                    })
                    .Where(x => x.TherapistName.Contains(name))
                    .ToList();

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

        public async Task<IActionResult> HandleGetTherapistBySpecificationId(Guid id)
        {
            try
            {
                var therapists = _context.TherapistSpecifications
                    .AsQueryable()
                    .Include(x => x.Specification)
                    .Include(x => x.Therapist).ThenInclude(xc => xc.Schedules)
                    .Where(x => x.SpecificationId == id && x.Therapist.Status == true)
                    .Select(x => new TherapistDTO
                    {
                        TherapistId = x.TherapistId,
                        TherapistName = x.Therapist.TherapistName,
                        Avatar = x.Therapist.Avatar,
                        Status = x.Therapist.Status,
                        ConsultationFee = x.Therapist.ConsultationFee,
                        Description = x.Therapist.Description,
                        MeetUrl = x.Therapist.MeetUrl,
                        Schedules = x.Therapist.Schedules.Select(s => new ScheduleResponseDTO
                        {
                            ScheduleId = s.ScheduleId,
                            Date = s.Date,
                            Slot = s.Slot,
                            Status = s.Status,
                        }).ToList(),
                        Specifications = x.Therapist.Specialty
                        .Where(m => m.Status == SpecificationStatusEnum.Active)
                        .Select(n => new SpecificationResponseDTO
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

        public async Task<IActionResult> GetTherapistByRating(decimal rating)
        {
            try
            {
                var avgRatings = GetAvgRating();

                const decimal tolerance = 0.5m;
                var lowerBound = rating - tolerance;

                var matchingTherapists = avgRatings
                    .Where(x => x.Value >= lowerBound)
                    .Select(x => new
                    {
                        Therapist = x.Key,
                        AverageRating = x.Value
                    })
                    .ToList();

                if (!matchingTherapists.Any())
                {
                    return NotFound($"No therapists found with an average rating close to {rating}.");
                }

                return Ok(matchingTherapists);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public Dictionary<Therapist, decimal> GetAvgRating()
        {
            var fb = _context.Feedbacks.AsQueryable();

            var rs = fb.Include(x => x.Booking)
                .ThenInclude(x => x.Therapist)
                .GroupBy(x => x.Booking.Therapist)
                .ToDictionary(key => key.Key, value => value.Average(x => x.Rating));
            return rs;
        }
    }
}
