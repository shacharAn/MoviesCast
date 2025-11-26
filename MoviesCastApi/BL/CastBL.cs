using MoviesCastApi.Dal;
using MoviesCastApi.Models;

namespace MoviesCastApi.BL
{
    public class CastBL
    {
        public static List<Cast> GetAllCast()
        {
            return CastDal.GetAllCast();
        }

        public static Cast InsertCast(Cast cast)
        {
            return CastDal.InsertCast(cast);
        }
    }
}
