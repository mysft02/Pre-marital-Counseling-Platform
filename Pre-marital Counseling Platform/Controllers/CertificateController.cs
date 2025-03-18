using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.Certificate;
using SWP391.Service;

namespace SWP391.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController (ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet("Get_All_Certificate")]
        public async Task<IActionResult> GetAllCertificate()
        {
            return await _certificateService.GetAllCertificate();
        }

        [HttpGet("Get_Certificate_By_Id")]
        public async Task<IActionResult> GetCertificateById(Guid id)
        {
            return await _certificateService.GetCertificateById(id);
        }

        [HttpGet("Get_Certificate_By_Therapist_Id")]
        public async Task<IActionResult> GetCertificateByTherapistId(Guid id)
        {
            return await _certificateService.GetCertificateByTherapistId(id);
        }

        [Authorize]
        [HttpPost("Create_Certificate")]
        public async Task<IActionResult> CreateCertificate([FromBody] CreateCertificateDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _certificateService.CreateCertificate(dto, userId);
        }

        [Authorize]
        [HttpPost("Update_Certificate")]
        public async Task<IActionResult> UpdateCertificate([FromBody] CreateCertificateDTO dto)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _certificateService.UpdateCertificate(dto, userId);
        }

        [Authorize]
        [HttpPost("Delete_Certificate")]
        public async Task<IActionResult> DeleteCertificate(Guid id)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            return await _certificateService.DeleteCertificate(id, userId);
        }
    }
}
