using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MangaReader.Models {
    public class Dialogue {
        public static async void Error(string content) {
            ContentDialog noWifiDialog = new ContentDialog() {
                Title = "Error",
                Content = content,
                PrimaryButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }
    }
}
