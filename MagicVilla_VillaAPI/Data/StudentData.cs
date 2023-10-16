using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class StudentData
    {
        public static List<StudentDTO> StudentsList = new List<StudentDTO>{
            new StudentDTO{Id = 1 , Name = "Suresh" },
            new StudentDTO{Id = 2 , Name = "Ramesh"}
        };
    }
}
