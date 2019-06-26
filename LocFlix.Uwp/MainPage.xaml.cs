using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI;
using Newtonsoft.Json;
using LocFlix.Uwp.ViewModels;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace LocFlix.Uwp
{
    public sealed partial class MainPage : Page
    {
        public List<string> Genres { get; set; } = new List<string>();

        public MovieViewModel ViewModel { get; set; } = new MovieViewModel();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void Date_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortMovies();
        }

        private void Search_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SortMovies();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortMovies();
        }

        public void SortMovies()
        {
            var fdt = new DateTime(int.Parse(From.Text), DateTime.Now.Month, DateTime.Now.Day);
            var tdt = new DateTime(int.Parse(To.Text), DateTime.Now.Month, DateTime.Now.Day);

            var sort = ViewModel.Movies.Where(x =>
                x.Title.ToLowerInvariant().Contains(Search.Text.ToLowerInvariant()));

            if (From.Text.Length == 4 || To.Text.Length == 4)
                sort = sort.Where(x => x.ReleaseDate != null && (new DateTime(x.ReleaseDate.Value.Year, DateTime.Now.Month,
                                                                              DateTime.Now.Day).Year > fdt.Year &&
                                                                          new DateTime(x.ReleaseDate.Value.Year, DateTime.Now.Month,
                                                                              DateTime.Now.Day).Year < tdt.Year));

            var acv = new AdvancedCollectionView(new ObservableCollection<Movie>(sort));

            var content = ((ComboBoxItem)Sort.SelectedItem)?.Content;

            if (content != null && content.ToString().Contains("Ascending"))
                acv.SortDescriptions.Add(new SortDescription("Title", SortDirection.Ascending));
            else if (content != null && content.ToString().Contains("Descending"))
                acv.SortDescriptions.Add(new SortDescription("Title", SortDirection.Descending));
            else
                acv.SortDescriptions.Add(new SortDescription("Title", SortDirection.Ascending));

            var abx = new List<Movie>(acv.ToList().OfType<Movie>());

            if (!Genres.Any())
            {
                AdaptiveGridViewMovies.ItemsSource = abx;
                return;
            }

            foreach (var i in abx.ToList())
                if (!i.Genres.Any(x => Genres.Any(y => y == x.Name)))
                    abx.Remove(i);

            AdaptiveGridViewMovies.ItemsSource = abx;
        }

        private void Western_OnClick(object sender, RoutedEventArgs e)
        {
            if (Western.IsChecked == true)
                Genres.Add("Western");
            else
                Genres.Remove("Western");

            SortMovies();
        }

        private void War_OnClick(object sender, RoutedEventArgs e)
        {
            if (War.IsChecked == true)
                Genres.Add("War");
            else
                Genres.Remove("War");

            SortMovies();
        }

        private void Thriller_OnClick(object sender, RoutedEventArgs e)
        {
            if (Thriller.IsChecked == true)
                Genres.Add("Thriller");
            else
                Genres.Remove("Thriller");

            SortMovies();
        }

        private void TvMovie_OnClick(object sender, RoutedEventArgs e)
        {
            if (TvMovie.IsChecked == true)
                Genres.Add("Tv Movie");
            else
                Genres.Remove("Tv Movie");

            SortMovies();
        }

        private void ScienceFiction_OnClick(object sender, RoutedEventArgs e)
        {
            if (ScienceFiction.IsChecked == true)
                Genres.Add("Science Fiction");
            else
                Genres.Remove("Science Fiction");

            SortMovies();
        }

        private void Romance_OnClick(object sender, RoutedEventArgs e)
        {
            if (Romance.IsChecked == true)
                Genres.Add("Romance");
            else
                Genres.Remove("Romance");

            SortMovies();
        }

        private void Mystery_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mystery.IsChecked == true)
                Genres.Add("Mystery");
            else
                Genres.Remove("Mystery");

            SortMovies();
        }

        private void Horror_OnClick(object sender, RoutedEventArgs e)
        {
            if (Horror.IsChecked == true)
                Genres.Add("Horror");
            else
                Genres.Remove("Horror");

            SortMovies();
        }

        private void History_OnClick(object sender, RoutedEventArgs e)
        {
            if (History.IsChecked == true)
                Genres.Add("History");
            else
                Genres.Remove("History");

            SortMovies();
        }

        private void Fantasy_OnClick(object sender, RoutedEventArgs e)
        {
            if (Fantasy.IsChecked == true)
                Genres.Add("Fantasy");
            else
                Genres.Remove("Fantasy");

            SortMovies();
        }

        private void Family_OnClick(object sender, RoutedEventArgs e)
        {
            if (Family.IsChecked == true)
                Genres.Add("Family");
            else
                Genres.Remove("Family");

            SortMovies();
        }

        private void Drama_OnClick(object sender, RoutedEventArgs e)
        {
            if (Drama.IsChecked == true)
                Genres.Add("Drama");
            else
                Genres.Remove("Drama");

            SortMovies();
        }

        private void Documentary_OnClick(object sender, RoutedEventArgs e)
        {
            if (Documentary.IsChecked == true)
                Genres.Add("Documentary");
            else
                Genres.Remove("Documentary");

            SortMovies();
        }

        private void Crime_OnClick(object sender, RoutedEventArgs e)
        {
            if (Crime.IsChecked == true)
                Genres.Add("Crime");
            else
                Genres.Remove("Crime");

            SortMovies();
        }

        private void Comedy_OnClick(object sender, RoutedEventArgs e)
        {
            if (Comedy.IsChecked == true)
                Genres.Add("Comedy");
            else
                Genres.Remove("Comedy");

            SortMovies();
        }

        private void Animation_OnClick(object sender, RoutedEventArgs e)
        {
            if (Animation.IsChecked == true)
                Genres.Add("Animation");
            else
                Genres.Remove("Animation");

            SortMovies();
        }

        private void Adventure_OnClick(object sender, RoutedEventArgs e)
        {
            if (Adventure.IsChecked == true)
                Genres.Add("Adventure");
            else
                Genres.Remove("Adventure");

            SortMovies();

        }

        private void Action_OnClick(object sender, RoutedEventArgs e)
        {
            if (Action.IsChecked == true)
                Genres.Add("Action");
            else
                Genres.Remove("Action");

            SortMovies();
        }

        private void AdaptiveGridViewMovies_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(MoviePage), JsonConvert.SerializeObject((Movie)e.ClickedItem));
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            Search.Text = "";
            Sort.SelectedIndex = 0;
            From.Text = "1900";
            To.Text = "2020";
            Western.IsChecked = false;
            War.IsChecked = false;
            Thriller.IsChecked = false;
            ScienceFiction.IsChecked = false;
            Romance.IsChecked = false;
            Mystery.IsChecked = false;
            Horror.IsChecked = false;
            History.IsChecked = false;
            Fantasy.IsChecked = false;
            Family.IsChecked = false;
            Drama.IsChecked = false;
            Documentary.IsChecked = false;
            Crime.IsChecked = false;
            Comedy.IsChecked = false;
            Animation.IsChecked = false;
            Adventure.IsChecked = false;
            Action.IsChecked = false;
            Genres.Clear();
        }

        private async void ReConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var cd = new ConfigContentDialog();
            await cd.ShowAsync();
        }
    }
}
