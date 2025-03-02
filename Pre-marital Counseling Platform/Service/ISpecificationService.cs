using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface ISpecificationService
    {
        Task<IActionResult> HandleCreateSpecification(SpecificationCreateDTO specificationCreateDTO);
        Task<IActionResult> HandleGetAllSpecifications();
        Task<IActionResult> HandleGetSpecificationById(Guid id);
        Task<IActionResult> HandleUpdateSpecification(SpecificationUpdateDTO specificationUpdateDTO);
        Task<IActionResult> HandleUpdateTherapistSpecification(TherapistSpecificationUpdateDTO therapistSpecificationUpdateDTO);
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
                var specifications = _context.Specifications
                    .Include(c => c.Therapists).ThenInclude(c => c.Therapist)
                    .Select(x => new SpecificationDTO
                    {
                        SpecificationId = x.SpecificationId,
                        Name = x.Name,
                        Description = x.Description,
                        Level = x.Level,
                        Therapists = x.Therapists.Select(c => c.Therapist).ToList()
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
                    .Select(x => new SpecificationDTO
                    {
                        SpecificationId = x.SpecificationId,
                        Name = x.Name,
                        Description = x.Description,
                        Level = x.Level,
                        Therapists = x.Therapists.Select(c => c.Therapist).ToList()
                    })
                    .FirstOrDefault(x => x.SpecificationId == id);

                return Ok(specification);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
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
                var thespe = new TherapistSpecification
                {
                    TherapistId = therapistSpecificationUpdateDTO.TherapistId,
                    SpecificationId = therapistSpecificationUpdateDTO.SpecificationId,
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
    }
}
