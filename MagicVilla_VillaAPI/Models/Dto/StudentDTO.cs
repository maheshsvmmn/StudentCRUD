namespace MagicVilla_VillaAPI.Models.Dto
{
    public class StudentDTO
    {
        public int Id { get; internal set; }

        public string Name { get; set; }
        public int Class { get; set; }

        public double Weight { get; set; }
        public DateTime CreatedAt { get; internal set; }



    }
}
