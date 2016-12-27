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
using System.Net.NetworkInformation;

namespace MangaReader.ViewModels {
    class MangaDetailViewModel : ViewModelBase {

        private MangaItem _mangaDetail;
        public MangaItem mangaDetail {
            get { return _mangaDetail; }
            set { Set(ref _mangaDetail, value); }
        }
        
        private string _mangaId;
        
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            _mangaId = parameter as string;
            Initialize(); 

            await Task.CompletedTask;
        }

        private async void Initialize() {
            //načtení detailu při otevření obrazovky
            try {
                mangaDetail = await MangaItem.GetDetailAsync(_mangaId);
            }
            catch (Exception e) {
                mangaDetail = null;
                if (NetworkInterface.GetIsNetworkAvailable()) {
                    Dialogue.Error("There was an error.");
                    Debug.WriteLine(e);
                } else {
                    Dialogue.Error("You are not connected to the Internet.");
                }
            }
        }

        public void ChapterSelected(object sender, ItemClickEventArgs e) {
            //event výběru kapitoly
            var _chapter = (ChapterListItem)e.ClickedItem;

            //připraví se základní informace o vybrané kapitole, aby se znovu nemusely na další stránce znovu stahovat
            string title = "Chapter " + _chapter.number;

            if(_chapter.title != "") {
                title = title + ": " + _chapter.title;
            }

            var _chapterView = new MangaChapterView(
                title,
                _chapter.id
                );
            //informace se uloží do classy a odešlou se jako parametr navigační metody
            NavigationService.Navigate(typeof(Views.ChapterPage), _chapterView);
        }
    }
}
