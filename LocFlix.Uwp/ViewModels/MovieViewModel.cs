using Newtonsoft.Json;
using LocFlix.Uwp.Singletons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Client;
using Windows.Storage;
using LocFlix.Uwp.Helpers;
using TMDbLib.Objects.Movies;

namespace LocFlix.Uwp.ViewModels
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

        public TMDbClient client;

        public Config Config { get; set; }

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
            DoStuff();
        }

        private async Task DoStuff()
        {
            if (!Singleton.DataRetrieved)
            {
                StorageFile file1 = await ApplicationData.Current.LocalFolder.GetFileAsync("config.json");
                string lines1 = await FileIO.ReadTextAsync(file1);

                Config config = JsonConvert.DeserializeObject<Config>(lines1);

                client = new TMDbClient(config.ApiKey);

                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("movies.json");
                string lines = await FileIO.ReadTextAsync(file);

                Movies = JsonConvert.DeserializeObject<ObservableCollection<Movie>>(lines);

                Singleton.DataRetrieved = true;
            }
        }

        private ICommand _newPath;

        public ICommand NewPath
        {
            get => _newPath ?? (_newPath = new Helpers.RelayCommand(async () =>
            {
                var file3 = await ApplicationData.Current.LocalFolder.TryGetItemAsync("config.json");
                if (file3 == null)
                {
                    await MessageDialogManager.MessageDialog("Oopsie! Please provide me a TMDB api key.");
                    return;
                }

                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add("*");

                StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {
                    Windows.Storage.AccessCache.StorageApplicationPermissions.
                    FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                }
                else
                {
                    return;
                }

                Movies = new ObservableCollection<Movie>();

                IReadOnlyList<StorageFile> fileEntries = await folder.GetFilesAsync();
                foreach (var fileEntry in fileEntries)
                    await ProcessFileAsync(fileEntry.Path);

                var filed = await ApplicationData.Current.LocalFolder.TryGetItemAsync("movies.json");
                if (filed != null)
                {
                    await filed.DeleteAsync();
                }

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("movies.json");
                await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(Movies));

                StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("config.json");
                if (file2 != null)
                {
                    string lines1 = await FileIO.ReadTextAsync(file2);
                    Config config = JsonConvert.DeserializeObject<Config>(lines1);

                    await file2.DeleteAsync();

                    var file1 = await ApplicationData.Current.LocalFolder.CreateFileAsync("config.json");
                    await FileIO.WriteTextAsync(file1, JsonConvert.SerializeObject(new Config { ApiKey = config.ApiKey, BasePath = Path.GetDirectoryName(Movies[0].LocalPath)}));
                }

                await MessageDialogManager.MessageDialog("Done! All movies have been processed.");
            }));
            set => _newPath = value;
        }

        public async Task ProcessFileAsync(string path)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));

            var file = Path.GetFileNameWithoutExtension(path);

            var search = client.SearchMovieAsync(file).Result;

            var movie = client.GetMovieAsync(
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
