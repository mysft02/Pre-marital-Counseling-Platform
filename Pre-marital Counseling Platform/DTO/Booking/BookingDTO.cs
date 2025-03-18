using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;
using SWP391.Migrations;

namespace SWP391.DTO
{
    public class BookingDTO
    {
        public Guid BookingId { get; set; }
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid ScheduleId { get; set; }
        public Feedback Feedback { get; set; }
        public Schedule Schedule { get; set; }
        public Therapist Therapist { get; set; }
        public BookingStatusEnum Status { get; set; }
        public string MeetUrl { get; set; }
    }

    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDTO>();
            CreateMap<Booking, BookingCreateDTO>().ReverseMap();
            CreateMap<Booking, BookingUpdateDTO>().ReverseMap();
        }
    }
}

public class BookingReturnDTO
{
    public Booking Booking { get; set; }
    public string Message { get; set; }
}

public class BookingResponseDTO
{
    public Booking Booking { get; set; }
    public string MemberName { get; set; }
    public string TherapistName { get; set; }
}
