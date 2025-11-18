using System.Linq;

namespace MoviesCastApi.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public int MovieId { get; set; }
        public string PhotoUrl { get; set; }

        public static List<Cast> Casts { get; } = new();

        public bool Insert()
        {
            if (Casts.Any(c => c.Id == this.Id)) return false;
            Casts.Add(this);
            return true;
        }

        public static List<Cast> Read() => Casts;
    }
}
