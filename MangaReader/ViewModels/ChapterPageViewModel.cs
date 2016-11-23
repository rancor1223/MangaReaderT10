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
using System.Diagnostics;
using Windows.Storage;

namespace MangaReader.ViewModels {
    public class ChapterPageViewModel : ViewModelBase {
        private List<MangaPage> _pageList;
        public List<MangaPage> pageList {
            get { return _pageList; }
            set { Set(ref _pageList, value); }
        }

        private string _chapterId { get; set; }


        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            _chapterId = parameter as string;
            Initialize();

            await Task.CompletedTask;
        }

        private async void Initialize() {
            pageList = await MangaChapterGet.GetAsync(_chapterId);
        }
        
    }
}

