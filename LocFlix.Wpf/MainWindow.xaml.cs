using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LocFlix.Wpf.ViewModels;
using TMDbLib.Objects.Movies;

namespace LocFlix.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Genres { get; set; } = new List<string>();

        public MovieViewModel ViewModel { get; set; } = new MovieViewModel();

        public MainWindow()
        {
            InitializeComponent();
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
            var sort = ViewModel.Movies.Where(x =>
                x.Title.ToLowerInvariant().Contains(Search.Text.ToLowerInvariant()));

            if (From.Text.Length == 4 || To.Text.Length == 4)
            {
                var fdt = new DateTime(int.Parse(From.Text), DateTime.Now.Month, DateTime.Now.Day);
                var tdt = new DateTime(int.Parse(To.Text), DateTime.Now.Month, DateTime.Now.Day);

                sort = sort.Where(x => x.ReleaseDate != null && (new DateTime(x.ReleaseDate.Value.Year, DateTime.Now.Month,
                                                                              DateTime.Now.Day).Year > fdt.Year &&
                                                                          new DateTime(x.ReleaseDate.Value.Year, DateTime.Now.Month,
                                                                              DateTime.Now.Day).Year < tdt.Year));
            }

            var content = ((ComboBoxItem)Sort.SelectedItem)?.Content;
            if (content != null && content.ToString().Contains("Ascending"))
                sort = sort.OrderBy(i => i.Title);
            else if (content != null && content.ToString().Contains("Descending"))
                sort = sort.OrderByDescending(i => i.Title);
            else
                sort = sort.OrderBy(i => i.Title);


            var abx = new List<Movie>(sort.ToList());

            if (!Genres.Any())
            {
                ItemsControlMovies.ItemsSource = abx;
                return;
            }

            foreach (var i in abx.ToList())
                if (!i.Genres.Any(x => Genres.Any(y => y == x.Name)))
                    abx.Remove(i);

            ItemsControlMovies.ItemsSource = abx;
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
    }
}
