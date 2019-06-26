using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using LocFlix.Wpf.Helpers;
using LocFlix.Wpf.Singletons;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace LocFlix.Wpf.ViewModels
{
    public class MovieViewModel : INotifyPropertyChanged
    {
        //| :--------------------------------------------------------: |
        //|                               | Min Res     | Max Res      |
        //| :--------------------------------------------------------: |
        //| poster   = Poster............ | 500 x 750   | 2000 x 3000  |
        //| backdrop = Fanart............ | 1280 x 720  | 3840 x 2160  |
        //| still    = TV Show Episode .. | 1280 x 720  | 3840 x 2160  |
        //| profile  = Actors Actresses.. | 300 x 450   | 2000 x 3000  |
        //| logo     = TMDb Logo......... | ?           | ?            |
        //| :--------------------------------------------------------: |

        //| :--------------------------------------------------: |
        //| poster   | backdrop | still    | profile  | logo     |
        //| :------: | :------: | :------: | :------: | :------: |
        //| -------- | -------- | -------- | w45      | w45      |
        //| w92      | -------- | w92      | -------- | w92      |
        //| w154     | -------- | -------- | -------- | w154     |
        //| w185     | -------- | w185     | w185     | w185     |
        //| -------- | w300     | w300     | -------- | w300     |
        //| w342     | -------- | -------- | -------- | -------- |
        //| w500     | -------- | -------- | -------- | w500     |
        //| -------- | -------- | -------- | h632     | -------- |
        //| w780     | w780     | -------- | -------- | -------- |
        //| -------- | w1280    | -------- | -------- | -------- |
        //| original | original | original | original | original |
        //| :--------------------------------------------------: |

        // Examples
        // https://image.tmdb.org/t/p/original/ + poster.FilePath (High resolution)
        // https://image.tmdb.org/t/p/w[Your_Custom_Width]/ + poster.FilePath (Custom resolution)
        // https://www.youtube.com/watch?v + video.Key (Trailers from youtube)

        public MovieSingleton Singleton = MovieSingleton.Instance;

        public TMDbClient Client = new TMDbClient("PLACEYOURTMDBAPIKEYHERE");

        public ObservableCollection<Movie> Movies
        {
            get => Singleton.Catalog;
            set
            {
                Singleton.Catalog = value;
                OnPropertyChanged();
            }
        }

        public MovieViewModel()
        {
            if (!Singleton.DataRetrieved)
            {
                var location = System.Reflection.Assembly.GetEntryAssembly()?.Location;
                var exeLocation = Path.GetDirectoryName(location);

                var moviesLocation = exeLocation + "\\Movies.json";

                if (!File.Exists(moviesLocation))
                    File.Create(moviesLocation).Close();

                var moviesString = File.ReadAllText(moviesLocation);


                Movies = JsonConvert.DeserializeObject<ObservableCollection<Movie>>(moviesString);

                Singleton.DataRetrieved = true;
            }
        }

        private ICommand _newPath;

        public ICommand NewPath
        {
            get => _newPath ?? (_newPath = new RelayCommand(async () =>
            {
                var folderPicker = new FolderBrowserDialog();
                folderPicker.ShowDialog();

                if (string.IsNullOrEmpty(folderPicker.SelectedPath))
                    return;

                Movies = new ObservableCollection<Movie>();

                var fileEntries = Directory.GetFiles(folderPicker.SelectedPath);
                foreach (var fileEntry in fileEntries)
                    await ProcessFileAsync(fileEntry);

                var location = System.Reflection.Assembly.GetEntryAssembly()?.Location;
                var exeLocation = Path.GetDirectoryName(location);

                var moviesLocation = exeLocation + "\\Movies.json";

                File.WriteAllText(moviesLocation, string.Empty);

                using (StreamWriter outputFile = new StreamWriter(moviesLocation))
                {
                    outputFile.Write(JsonConvert.SerializeObject(Movies));
                }
            }));
            set => _newPath = value;
        }

        public async Task ProcessFileAsync(string path)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));

            var file = Path.GetFileNameWithoutExtension(path);

            var search = Client.SearchMovieAsync(file).Result;

            var movie = Client.GetMovieAsync(
                search.Results[0].Id,
                MovieMethods.Images |
                MovieMethods.Videos |
                MovieMethods.Reviews |
                MovieMethods.Keywords |
                MovieMethods.Similar |
                MovieMethods.Credits |
                MovieMethods.Releases
                ).Result;

            movie.LocalTitle = Path.GetFileName(path);
            movie.LocalPath = path;

            Movies.Add(movie);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
