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
using System.Net.NetworkInformation;

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

        private bool _loading;
        public bool loading {
            get { return _loading; }
            set { Set(ref _loading, value); }
        }

        public MainPageViewModel() {
            loading = false;
            mangaList = new List<MangaItem>();


            //Initialize();
        }

        private async void Initialize() {
            try {
                mangaList = await MangaItem.GetListAsync();
            }
            catch (Exception e) {
                mangaList = null;
                if (NetworkInterface.GetIsNetworkAvailable()) {
                    Dialogue.Error("There was an error.");
                    Debug.WriteLine(e);
                } else {
                    Dialogue.Error("You are not connected to the Internet.");
                }
            }
        }

        public async void MainSearchSubmitted() {
            loading = true;

            try {
                mangaList = await MangaItem.GetListAsync(_mainSearchText);
            }
            catch (Exception e) {
                mangaList = null;
                if (NetworkInterface.GetIsNetworkAvailable()) {
                    Dialogue.Error("There was an error.");
                    Debug.WriteLine(e);
                } else {
                    Dialogue.Error("You are not connected to the Internet.");
                }
            }
            finally {
                loading = false;
            }
        }

        public void MangaSelected(object sender, ItemClickEventArgs e) {
            var mangaItem = (MangaItem)e.ClickedItem;
            NavigationService.Navigate(typeof(Views.MangaDetail), mangaItem.id);
        }
    }
}

