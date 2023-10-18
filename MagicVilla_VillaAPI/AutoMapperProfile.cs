using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Students_API.Models;
using Students_API.Models.Dto;

namespace Students_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentDTO, Student>();
            CreateMap<Teacher, TeacherDto>();
            CreateMap<TeacherDto, Teacher>();
        }
    }
}
