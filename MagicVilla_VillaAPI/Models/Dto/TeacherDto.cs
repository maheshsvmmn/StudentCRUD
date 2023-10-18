namespace Students_API.Models.Dto
{
    public class TeacherDto
    {
        public int Id { get; internal set; }
        public string Name { get; set; }

        public string Subject { get; set; }

        public int Salary { get; set; }
    }
}
