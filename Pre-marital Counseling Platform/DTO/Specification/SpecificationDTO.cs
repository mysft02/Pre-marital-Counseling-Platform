using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class SpecificationDTO
    {
        public Guid SpecificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public List<Therapist> Therapists { get; set; } 
    }

    public class SpecificationResponseListDTO
    {
        public Guid SpecificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Therapist> Therapists { get; set; }
    }

    public class SpecificationCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
    }

    public class SpecificationUpdateDTO
    {
        public Guid SpecificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
    }

    public class TherapistSpecificationUpdateDTO
    {
        public Guid TherapistId { get; set; }
        public Guid SpecificationId { get; set; }
    }

    public class SpecificationResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public SpecificationStatusEnum Status { get; set; }
    }

    public class SpecificationProfile : Profile
    {
        public SpecificationProfile()
        {
            CreateMap<Specification, SpecificationResponseListDTO>().ReverseMap();
            CreateMap<SpecificationCreateDTO, Specification>().ReverseMap();
            CreateMap<SpecificationUpdateDTO, Specification>().ReverseMap();
        }
    }
}
