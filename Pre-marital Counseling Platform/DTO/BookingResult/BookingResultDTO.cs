using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class BookingResultDTO
    {
        public Guid BookingResultId { get; set; }
        public Guid BookingId { get; set; }
        public string Description { get; set; }
    }

    public class BookingResultProfile : Profile
    {
        public BookingResultProfile()
        {
            CreateMap<BookingResult, BookingResultDTO>().ReverseMap();
            CreateMap<BookingResult, BookingResultCreateDTO>().ReverseMap();
        }
    }
}
