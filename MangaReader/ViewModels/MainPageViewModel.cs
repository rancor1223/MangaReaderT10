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
            
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) {
                mangaList = new List<MangaItem>();
                mangaList.Add(new MangaItem("Test Manga", "https://cdn.mangaeden.com/mangasimg/d6/d6e8037969cc382f33e5a153871b522631f85eed538bd4d77cc2f928.png"));
                mangaList.Add(new MangaItem("Test Manga", "https://cdn.mangaeden.com/mangasimg/cd/cdf51728888ce531ac7138ef431330708675a61bbcdc767a3103efbd.jpg"));
                mangaList.Add(new MangaItem("Test Manga", "https://cdn.mangaeden.com/mangasimg/5d/5d5d66d58bc13129ca64fcb75e1cd1b4af25ce6b53cf7ff057ade7b3.jpg"));
            }
            

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

