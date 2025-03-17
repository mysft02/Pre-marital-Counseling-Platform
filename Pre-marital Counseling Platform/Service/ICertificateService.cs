using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.DTO.Certificate;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface ICertificateService
    {
        Task<IActionResult> GetAllCertificate();
        Task<IActionResult> GetCertificateById(Guid id);
        Task<IActionResult> CreateCertificate(CreateCertificateDTO dto, string userId);
        Task<IActionResult> UpdateCertificate(CreateCertificateDTO dTO, string userId);
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
                List<CertificateDTO> list = _mapper.Map<List<CertificateDTO>>(_context.Certificates.ToList());
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
                var certificate = _mapper.Map<CertificateDTO>(_context.Certificates.Where(x => x.CertificateId == id).FirstOrDefault());
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
                var nCertificate = _mapper.Map<Certificate>(dto);
                nCertificate.CertificateId = Guid.NewGuid();

                _context.Certificates.Add(nCertificate);
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

        public async Task<IActionResult> UpdateCertificate(CreateCertificateDTO dTO, string userId)
        {
            try
            {
                var certificate = _mapper.Map<Certificate>(dTO);
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
    }
}
