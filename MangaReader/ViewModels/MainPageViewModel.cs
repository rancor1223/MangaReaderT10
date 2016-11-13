using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using MangaReader.Models;
using MangaReader.Services;

namespace MangaReader.ViewModels {
    public class MainPageViewModel : ViewModelBase {
        private ObservableCollection<MangaListItem> _mangaList;
        public ObservableCollection<MangaListItem> mangaList {
            get { return _mangaList; }
            set { Set(ref _mangaList, value); }
        }

        private string _mainSearchText;
        public string mainSearchText {
            get { return _mainSearchText; }
            set { Set(ref _mainSearchText, value); } 
        }

        private string _selectedManga;
        public string selectedManga {
            get { return _selectedManga; }
            set { Set(ref _selectedManga, value); }
        }
        //^^ this isn't getting set

        public MainPageViewModel() {
            _mangaList = new ObservableCollection<MangaListItem>();
            mangaList = new ObservableCollection<MangaListItem>();

            Initialize();
        }

        private async void Initialize() {
            await MangaApi.PopulateMangaListAsync(_mangaList);
        }

        public async void MainSearchSubmitted() {
            await MangaApi.PopulateMangaListAsync(_mangaList, _mainSearchText);
        }

        public void MangaSelected() {
            NavigationService.Navigate(typeof(Views.MangaDetail), "4e70e9f6c092255ef7004336");
            //placeholder value "4e70e9f6c092255ef7004336"
        }

    }
}

