using System;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace MangaReader.Views {
    public sealed partial class MainPage : Page {
        public MainPage() {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
    }
}
