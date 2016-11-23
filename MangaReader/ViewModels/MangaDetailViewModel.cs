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
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp;
using System.Windows.Input;

namespace MangaReader.ViewModels {
    class MangaDetailViewModel : ViewModelBase {

        private MangaItem _mangaDetail;
        public MangaItem mangaDetail {
            get { return _mangaDetail; }
            set { Set(ref _mangaDetail, value); }
        }

        private bool _isFavourite;
        public bool isFavourite {
            get { return _isFavourite; }
            set { Set(ref _isFavourite, value); }
        }
        private string _mangaId;

        public MangaDetailViewModel() {
            isFavourite = false;
            var helper = new LocalObjectStorageHelper();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            _mangaId = parameter as string;
            Initialize();

            await Task.CompletedTask;
        }

        private async void Initialize() {
            mangaDetail = await MangaDetailGet.GetAsync(_mangaId);
        }

        public void ChapterSelected(object sender, ItemClickEventArgs e) {
            var _chapterId = (ChapterListItem)e.ClickedItem;
            NavigationService.Navigate(typeof(Views.ChapterPage), _chapterId.id);
        }

        public void MangaFavourited() {
            isFavourite = true;
            
        }

        public void MangaUnfavourited() {
            isFavourite = false;
        }
        
    }
}
