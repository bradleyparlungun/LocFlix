using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LocFlix.Uwp
{
    public sealed partial class ConfigContentDialog : ContentDialog
    {
        public ConfigContentDialog()
        {
            this.InitializeComponent();

            DoStuff();
        }

        public async void DoStuff()
        {
            var filed = await ApplicationData.Current.LocalFolder.TryGetItemAsync("config.json");
            if (filed != null)
            {
                StorageFile file1 = await ApplicationData.Current.LocalFolder.GetFileAsync("config.json");
                string lines1 = await FileIO.ReadTextAsync(file1);

                ApiKey.Text = JsonConvert.DeserializeObject<Config>(lines1).ApiKey;
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            cd.Hide();
        }

        private async void Save_OnClick(object sender, RoutedEventArgs e)
        {
            Config config;

            var filed = await ApplicationData.Current.LocalFolder.TryGetItemAsync("config.json");

            if (filed != null)
            {
                StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("config.json");

                string lines1 = await FileIO.ReadTextAsync(file2);
                config = JsonConvert.DeserializeObject<Config>(lines1);

                await file2.DeleteAsync();

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("config.json");
                await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(new Config { ApiKey = ApiKey.Text, BasePath = config.BasePath }));
            }
            else
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("config.json");
                await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(new Config { ApiKey = ApiKey.Text}));
            }

            
            cd.Hide();
        }
    }

    public class Config
    {
        public string ApiKey { get; set; }
        public string BasePath { get; set; }
    }
}
