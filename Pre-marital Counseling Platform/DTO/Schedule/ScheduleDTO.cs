﻿using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class ScheduleDTO
    {
        public Guid ScheduleId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public ScheduleStatusEnum Status { get; set; }
    }

    public class ScheduleResponseDTO
    {
        public Guid ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public ScheduleStatusEnum Status { get; set; }
    }

    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Schedule, ScheduleDTO>();
            CreateMap<Schedule, ScheduleCreateDTO>().ReverseMap();
            CreateMap<Schedule, ScheduleUpdateDTO>().ReverseMap();
        }
    }
}
