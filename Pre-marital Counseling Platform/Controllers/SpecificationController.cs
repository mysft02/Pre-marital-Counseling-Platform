using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecificationService _specificationService;
        private readonly IConfiguration _config;

        public SpecificationController(IConfiguration config, ISpecificationService specificationService)
        {
            _config = config;
            _specificationService = specificationService;
        }

        [HttpGet("Get_All_Specification")]
        public async Task<IActionResult> GetAllSpecifications()
        {

            return await _specificationService.HandleGetAllSpecifications();
        }

        [HttpGet("Get_Specification_By_Id")]
        public async Task<IActionResult> GetSpecificationById([FromQuery] Guid id)
        {

            return await _specificationService.HandleGetSpecificationById(id);
        }

        [HttpPost("Create_Specification")]
        public async Task<IActionResult> CreateSpecification([FromBody] SpecificationCreateDTO specificationCreateDTO)
        {

            return await _specificationService.HandleCreateSpecification(specificationCreateDTO);
        }

        [HttpPost("Update_Specification")]
        public async Task<IActionResult> UpdateSpecification([FromBody] SpecificationUpdateDTO specificationUpdateDTO)
        {

            return await _specificationService.HandleUpdateSpecification(specificationUpdateDTO);
        }

        [HttpPost("Update_Therapist_Specification")]
        public async Task<IActionResult> UpdateTherapistSpecification([FromBody] TherapistSpecificationUpdateDTO therapistSpecificationUpdateDTO)
        {

            return await _specificationService.HandleUpdateTherapistSpecification(therapistSpecificationUpdateDTO);
        }
    }
}
