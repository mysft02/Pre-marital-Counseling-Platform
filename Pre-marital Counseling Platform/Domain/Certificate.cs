﻿namespace SWP391.Domain
{
    public class Certificate
    {
        public Guid CertificateId { get; set; }
        public Guid TherapistId { get; set; }
        public string CertificateName { get; set; }
        public string CertificateUrl { get; set; }
        public Therapist Therapist { get; set; }
    }
}
