using AutoMapper;
using SWP391.DTO;   

namespace SWP391.DTO
{
    public class CertificateDTO
    {
        public Guid CertificateId { get; set; }
        public Guid TherapistId { get; set; }
        public string CertificateName { get; set; }
        public string CertificateUrl { get; set; }
    }

    public class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            CreateMap<SWP391.Domain.Certificate, CertificateDTO>().ReverseMap();
            CreateMap<CreateCertificateDTO, SWP391.Domain.Certificate>().ReverseMap();
        }
    }
}
