﻿namespace SWP391.DTO
{
    public class UpdateCertificateDTO
    {
        public Guid CertificateId { get; set; }
        public Guid TherapistId { get; set; }
        public string CertificateName { get; set; }
        public string CertificateUrl { get; set; }
    }
}
