using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;
using static SWP391.Service.SpecificationService;

namespace SWP391.Service
{
    public interface ISpecificationService
    {
        Task<IActionResult> HandleCreateSpecification(SpecificationCreateDTO specificationCreateDTO);
        Task<IActionResult> HandleGetAllSpecifications();
        Task<IActionResult> HandleGetAllSpecificationsWithLevel();
        Task<IActionResult> HandleGetSpecificationById(Guid id);
        Task<IActionResult> HandleUpdateTheSpeStatus(UpdateTheSpeDTO updateTheSpeDTO);
        Task<IActionResult> HandleUpdateSpecification(SpecificationUpdateDTO specificationUpdateDTO);
        Task<IActionResult> HandleUpdateTherapistSpecification(TherapistSpecificationUpdateDTO therapistSpecificationUpdateDTO);
        Task<IActionResult> HandleGetSpecificationByTherapistId(Guid id);
    }

    public class SpecificationService : ControllerBase, ISpecificationService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public SpecificationService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleGetAllSpecifications()
        {
            try
            {
                var specifications = _context.TherapistSpecifications
                    .AsQueryable()
                    .Include(x => x.Therapist)/*.ThenInclude(xc => xc.Schedules)*/
                    .Where(x => x.Therapist.Status == true)
                    .GroupBy(x => x.Specification.Name)
                    .Select(c => new TestResponseDTO
                    {
                        SpecificationName = c.Key,
                        Therapists = c.Select(m => m.Therapist).Distinct().ToList()
                    })
                    .ToList();

                return Ok(specifications);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public class TestResponseDTO
        {
            public string SpecificationName { get; set; }
            public List<Therapist> Therapists { get; set; }
        }

        public async Task<IActionResult> HandleGetAllSpecificationsWithLevel()
        {
            try
            {
                var specifications = _context.Specifications
                    .Include(c => c.Therapists).ThenInclude(c => c.Therapist)
                    .Select(x => new SpecificationDTO
                    {
                        SpecificationId = x.SpecificationId,
                        Name = x.Name,
                        Description = x.Description,
                        Level = x.Level,
                        Therapists = x.Therapists
                        .Select(c => c.Therapist)
                        .ToList()
                    })
                    .ToList();

                return Ok(specifications);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetSpecificationById(Guid id)
        {
            try
            {
                var specification = _context.Specifications
                    .Include(c => c.Therapists)
                    .Select(x => new SpecificationResponseListDTO
                    {
                        SpecificationId = x.SpecificationId,
                        Name = x.Name,
                        Description = x.Description,
                        Therapists = x.Therapists
                        .Where(c => c.Specification.Name == x.Name)
                        .Select(c => c.Therapist)
                        
                        .ToList()
                    })
                    .FirstOrDefault(x => x.SpecificationId == id);

                return Ok(specification);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetSpecificationByTherapistId(Guid id)
        {
            try
            {
                var specification = _context.TherapistSpecifications
                    .Include(c => c.Specification)
                    .Include(c => c.Therapist)
                    .Where(x => x.Therapist.TherapistId == id && x.Status == SpecificationStatusEnum.Active)
                    .Select(x => new TempResponseDTO
                    {
                        SpecificationId = x.SpecificationId,
                        Name = x.Specification.Name,
                        Description = x.Specification.Description,
                        Level = x.Specification.Level
                    })
                    .ToList();

                return Ok(specification);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public class TempResponseDTO
        {
            public Guid SpecificationId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Level { get; set; }
        }

        public async Task<IActionResult> HandleCreateSpecification(SpecificationCreateDTO specificationCreateDTO) 
        { 
            try
            {
                var specification = _mapper.Map<Specification>(specificationCreateDTO);
                _context.Specifications.Add(specification);

                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(specification);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateSpecification(SpecificationUpdateDTO specificationUpdateDTO)
        {
            try
            {
                var specification = _mapper.Map<Specification>(specificationUpdateDTO);

                _context.Specifications.Update(specification);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(specification);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateTherapistSpecification(TherapistSpecificationUpdateDTO therapistSpecificationUpdateDTO)
        {
            try
            {
                var query = _context.TherapistSpecifications.AsQueryable();

                if (query.Any(x => x.TherapistId == therapistSpecificationUpdateDTO.TherapistId && x.SpecificationId == therapistSpecificationUpdateDTO.SpecificationId))
                {
                    return BadRequest("Already exist");
                }
                var thespe = new TherapistSpecification
                {
                    TherapistId = therapistSpecificationUpdateDTO.TherapistId,
                    SpecificationId = therapistSpecificationUpdateDTO.SpecificationId,
                    Status = SpecificationStatusEnum.Pending
                };

                _context.TherapistSpecifications.Add(thespe);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(thespe);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public class UpdateTheSpeDTO
        {
            public Guid TherapistId { get; set; }
            public Guid SpecificationId { get; set; }
            public SpecificationStatusEnum Status { get; set; }
        }


        public async Task<IActionResult> HandleUpdateTheSpeStatus(UpdateTheSpeDTO updateTheSpeDTO)
        {
            try
            {
                var theSpe = _context.TherapistSpecifications
                    .FirstOrDefault(x => x.TherapistId == updateTheSpeDTO.TherapistId && x.SpecificationId == updateTheSpeDTO.SpecificationId);
                if (theSpe == null)
                {
                    return BadRequest("Not found");
                }
                theSpe.Status = updateTheSpeDTO.Status;

                _context.TherapistSpecifications.Update(theSpe);
                if (_context.SaveChanges() > 0)
                {
                    return Ok(theSpe);
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
