using System.Collections.ObjectModel;
using TMDbLib.Objects.Movies;

namespace LocFlix.Wpf.Singletons
{
    public class MovieSingleton
    {
        private static MovieSingleton _instance = new MovieSingleton();

        public static MovieSingleton Instance => _instance ?? (_instance = new MovieSingleton());

        public ObservableCollection<Movie> Catalog { get; set; }

        public bool DataRetrieved { get; set; } = false;
    }
}
