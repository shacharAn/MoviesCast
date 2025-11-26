using System.Linq;

namespace MoviesCastApi.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Role { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string PhotoUrl { get; set; }

    }
}
