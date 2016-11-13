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
    class MangaDetailViewModel : ViewModelBase {

        private MangaDetail _mangaDetail;
        public MangaDetail mangaDetail {
            get { return _mangaDetail; }
            set { Set(ref _mangaDetail, value); }
        }

        private string _mangaId;
        public string mangaId {
            get { return _mangaId; }
            set { Set(ref _mangaId, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            _mangaId = parameter as string;

            await Task.CompletedTask;
        }

        public MangaDetailViewModel() {
            _mangaDetail = new MangaDetail();
            mangaDetail = new MangaDetail();

            Initialize();
        }

        private async void Initialize() {
            await MangaApi.PopulateMangaDetailAsync(_mangaDetail, "4e70e9f6c092255ef7004336");
            //currently using placeholder value
        }

    }
}
