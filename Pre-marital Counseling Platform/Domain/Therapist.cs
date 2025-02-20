﻿namespace SWP391.Domain
{
    public class Therapist : BaseEntities
    {
        public Guid TherapistId { get; set; }
        public string Description { get; set; }
        public decimal ConsultationFee { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
