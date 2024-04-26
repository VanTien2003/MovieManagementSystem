using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class MovieTypeConverter
    {
        public MovieTypeConverter(){}

        public DataResponseMovieType EntityToDTO(MovieType movieType)
        {
            return new DataResponseMovieType()
            {
                MovieTypeName = movieType.MovieTypeName
            };
        }
    }
}
