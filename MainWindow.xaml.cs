// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Newtonsoft.Json;
using System.Diagnostics;
using WinRT;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Windows.Foundation.Diagnostics;
using Newtonsoft.Json.Bson;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DiscordEmoteApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //LoadAssets();

        }

        public List<DiscordAsset> models = new();
        public ObservableCollection<DiscordAsset> Data { get; set; } = new ObservableCollection<DiscordAsset>();

        public IServiceProvider container = (Application.Current as App).Container;



        public void LoadAssets()
        {
            string filepath = $"{Windows.ApplicationModel.Package.Current.InstalledPath}/Assets/data.json";

            var contents = File.ReadAllText(filepath);

            models = JsonConvert.DeserializeObject<List<DiscordAsset>>(contents);

        }

        public async Task LoadDownloadedAssets()
        {
            var service = (Application.Current as App).Container.GetService<Downloader>();
            models = await service.ReademotesAsync();
                
        }


        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (models.Count == 0)
                {
                    await LoadDownloadedAssets();
                }

                Data.Clear();
                var modelsfilt = models.Where(x => x.name.ToLower().Contains(Input.Text.ToLower())
                                              || x.tags.ToLower().Contains(Input.Text.ToLower()));
                foreach (var item in modelsfilt)
                {
                    Data.Add(item);
                }
            }
            

        }

        private void CopyToClipboard(string text)
        {
            var package = new DataPackage();
            package.SetText(text);
            Clipboard.SetContent(package);
        }


        private void BasicGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var mod = (e.ClickedItem as DiscordAsset);
            CopyToClipboard(mod.ClipboardText);

        }

        private void WebView2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var uri = (e.OriginalSource as WebView2).Source;
            CopyToClipboard(uri.ToString());

        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var service = container.GetService<Downloader>();
            DownloadProgress.IsActive = true;
            DownloadProgress.Visibility = Visibility.Visible;
            var token = container.GetService<AppConfig>().ConfigurationRoot["token"];
            await service.DownloadfilesAsync(token);
            DownloadProgress.IsActive = false;
            DownloadProgress.Visibility = Visibility.Collapsed;

        }
    }
}
