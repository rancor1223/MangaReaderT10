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
using Microsoft.Toolkit.Uwp;
using System.Windows.Input;
using System.Diagnostics;

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
        private LocalObjectStorageHelper _objectStorage = new LocalObjectStorageHelper();

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
            mangaDetail = await MangaItem.GetDetailAsync(_mangaId);

            var o = new List<MangaItem> { mangaDetail };
            await _objectStorage.SaveFileAsync("favourites", o);
            
        }

        public void ChapterSelected(object sender, ItemClickEventArgs e) {
            var _chapter = (ChapterListItem)e.ClickedItem;

            var _chapterView = new MangaChapterView();
            _chapterView.chapterId = _chapter.id;
            if(_chapterView.title == "") {
                _chapterView.title = "Chapter " + _chapter.number;
            } else {
                _chapterView.title = _chapter.title;
            }
            _chapterView.title = _chapter.title;

            NavigationService.Navigate(typeof(Views.ChapterPage), _chapterView);
        }

        public async void MangaFavourited() {
            if (await _objectStorage.FileExistsAsync("favourites")) {
                var result = await _objectStorage.ReadFileAsync<List<MangaItem>>("favourites");
                result.Add(mangaDetail);
                await _objectStorage.SaveFileAsync("favourites", result);

                isFavourite = true;
            } else {
                Debug.WriteLine("problem reading favourites");
            }
        }

        public void MangaUnfavourited() {
            isFavourite = false;
        }
        
    }
}
