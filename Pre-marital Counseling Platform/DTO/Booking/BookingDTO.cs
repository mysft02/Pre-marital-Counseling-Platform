using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class BookingDTO
    {
        public Guid BookingId { get; set; }
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid ScheduleId { get; set; }
        public BookingStatusEnum Status { get; set; }
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
