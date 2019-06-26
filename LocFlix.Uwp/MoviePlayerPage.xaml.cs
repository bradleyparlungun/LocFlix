using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using TMDbLib.Objects.Movies;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LocFlix.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoviePlayerPage : Page
    {
        public MoviePlayerPage()
        {
            this.InitializeComponent();
        }

        public Movie Movie;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.InitializeComponent();

            Movie = JsonConvert.DeserializeObject<Movie>((string)e.Parameter);

            Backdrop.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + Movie.Images.Backdrops[0].FilePath));


            SetLocalMedia();
        }

        private async void SetLocalMedia()
        {
            StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("config.json");

            string lines1 = await FileIO.ReadTextAsync(file2);
            Config config = JsonConvert.DeserializeObject<Config>(lines1);


            var folder = await StorageFolder.GetFolderFromPathAsync(config.BasePath);
            var file = await folder.GetFileAsync(Movie.LocalTitle);

            if (file != null)
            {
                mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);

                mediaPlayer.MediaPlayer.Play();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = null;
            Frame.Navigate(typeof(MoviePage), JsonConvert.SerializeObject(Movie));
        }
    }
}
