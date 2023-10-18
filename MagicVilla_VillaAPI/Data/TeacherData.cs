using Students_API.Models;

namespace Students_API.Data
{
    public static class TeacherData
    {
        public static List<Teacher> TeachersList = new List<Teacher>
        {
            new Teacher{Id = 1 , Name = "Rakesh" , Subject = "Math" , Salary = 45000 , HiringDate = DateTime.Now , Rating = 4.5 },
            new Teacher{Id = 2 , Name = "Ranjan" , Subject = "Science" , Salary = 34000 , HiringDate = DateTime.Now , Rating = 3.4 }
        };
    }
}
