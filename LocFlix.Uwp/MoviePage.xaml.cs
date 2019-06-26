using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MoviePage : Page
    {
        public Movie Movie { get; set; }

        public MoviePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Movie movie = JsonConvert.DeserializeObject<Movie>((string)e.Parameter);

            Movie = movie;

            Poster.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + movie.Images.Posters[0].FilePath));
            Backdrop.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + movie.Images.Backdrops[0].FilePath));
            Similar1.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + movie.Similar.Results[0].PosterPath));
            Similar2.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + movie.Similar.Results[1].PosterPath));
            Similar3.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + movie.Similar.Results[2].PosterPath));
            Similar4.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/original" + movie.Similar.Results[3].PosterPath));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void PlayMovie_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MoviePlayerPage), JsonConvert.SerializeObject(Movie));
        }
    }
}
