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

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            _mangaId = parameter as string;
            Initialize();

            await Task.CompletedTask;
        }

        private async void Initialize() {
            await MangaApi.PopulateMangaDetailAsync(_mangaDetail, _mangaId);
            //^^ this doesn't update the view
        }

    }
}
