﻿namespace SWP391.DTO.Certificate
{
    public class CreateCertificateDTO
    {
        public Guid CertificateId { get; set; }
        public Guid TherapistId { get; set; }
        public string CertificateName { get; set; }
        public string CertificateUrl { get; set; }
    }
}
