using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using MangaReader.Models;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.Storage;

namespace MangaReader.ViewModels {
    public class MainPageViewModel : ViewModelBase {
        private List<MangaItem> _mangaList;
        public List<MangaItem> mangaList {
            get { return _mangaList; }
            set { Set(ref _mangaList, value); }
        }

        private string _mainSearchText;
        public string mainSearchText {
            get { return _mainSearchText; }
            set { Set(ref _mainSearchText, value); }
        }


        public MainPageViewModel() {
            _mangaList = new List<MangaItem>();
            mangaList = new List<MangaItem>();

            Initialize();
        }

        private async void Initialize() {
            mangaList = await MangaItem.GetListAsync();
        }

        public async void MainSearchSubmitted() {
            mangaList = await MangaItem.GetListAsync(_mainSearchText);
        }

        public void MangaSelected(object sender, ItemClickEventArgs e) {
            var mangaItem = (MangaItem)e.ClickedItem;
            NavigationService.Navigate(typeof(Views.MangaDetail), mangaItem.id);
        }
    }
}

