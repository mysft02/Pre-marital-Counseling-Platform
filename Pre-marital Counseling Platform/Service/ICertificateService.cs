using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface ICertificateService
    {
        Task<IActionResult> GetAllCertificate();
        Task<IActionResult> GetCertificateById(Guid id);
        Task<IActionResult> CreateCertificate(CreateCertificateDTO dto, string userId);
        Task<IActionResult> UpdateCertificate(UpdateCertificateDTO dTO, string userId);
        Task<IActionResult> GetCertificateByTherapistId(Guid therapistId);
        Task<IActionResult> DeleteCertificate(Guid certificateId, string userId);
    }

    public class CertificateService : ControllerBase, ICertificateService
    {

        private readonly IMapper _mapper;
        private readonly PmcsDbContext _context;

        public CertificateService(IMapper mapper, PmcsDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllCertificate()
        {
            try
            {
                List<CertificateDTO> list = _mapper.Map<List<CertificateDTO>>(_context.Certificates.Where(x => x.Status == CertificateStatusEnum.ACTIVE).ToList());
                if (list == null)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetCertificateById(Guid id)
        {
            try
            {
                var certificate = _mapper.Map<CertificateDTO>(_context.Certificates.Where(x => x.CertificateId == id && x.Status == CertificateStatusEnum.ACTIVE).FirstOrDefault());
                if (certificate == null)
                {
                    return NotFound();
                }
                return Ok(certificate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> CreateCertificate(CreateCertificateDTO dto, string userId)
        {
            try
            {
                var query = _context.Certificates.AsQueryable();

                var check = query.FirstOrDefault(x => x.TherapistId == dto.TherapistId && x.Status == CertificateStatusEnum.ACTIVE);
                if (check != null)
                {
                    check.CertificateUrl = dto.CertificateUrl;
                    check.CertificateName = dto.CertificateName;

                    _context.Certificates.Update(check);
                }
                else
                {
                    var nCertificate = _mapper.Map<Certificate>(dto);
                    nCertificate.Status = CertificateStatusEnum.ACTIVE;

                    _context.Certificates.Add(nCertificate);
                }
                
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok("Create Successfully");
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> UpdateCertificate(UpdateCertificateDTO dTO, string userId)
        {
            try
            {
                var certificate = _context.Certificates.FirstOrDefault(x => x.CertificateId == dTO.CertificateId);
                certificate.CertificateUrl = dTO.CertificateUrl;
                certificate.CertificateName = dTO.CertificateName;
                _context.Certificates.Update(certificate);
                if(_context.SaveChanges() > 0)
                {
                    return Ok("Update successfully");
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetCertificateByTherapistId(Guid therapistId)
        {
            try
            {
                var certificate = _mapper.Map<CertificateDTO>(_context.Certificates.Where(x => x.TherapistId == therapistId && x.Status == CertificateStatusEnum.ACTIVE).FirstOrDefault());
                if (certificate == null)
                {
                    return NotFound();
                }
                return Ok(certificate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> DeleteCertificate(Guid certificateId, string userId)
        {
            try
            {
                var certificate = _context.Certificates.FirstOrDefault(x => x.CertificateId == certificateId);
                if (certificate == null)
                {
                    return NotFound("Certificate not found.");
                }

                certificate.Status = CertificateStatusEnum.INACTIVE;

                _context.Certificates.Update(certificate);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return Ok("Certificate deleted successfully.");
                }
                else
                {
                    return BadRequest("Failed to delete certificate.");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
