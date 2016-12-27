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
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Net.NetworkInformation;

namespace MangaReader.ViewModels {
    public class ChapterPageViewModel : ViewModelBase {
        private List<MangaPage> _pageList;
        public List<MangaPage> pageList {
            get { return _pageList; }
            set { Set(ref _pageList, value); }
        }

        //page counter
        private int _currentPage;
        public int currentPage {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        //image size
        private double _pageWidth;
        public double pageWidth {
            get { return _pageWidth; }
            set { Set(ref _pageWidth, value); }
        }

        private double _pageHeight;
        public double pageHeight {
            get { return _pageHeight; }
            set { Set(ref _pageHeight, value); }
        }

        //page size
        private double _windowWidth;
        private double _windowHeight;

        //command bar title
        private string _title;
        public string title {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        //string zobrazující aktuální stránku, zdormátovaný
        private string _pageMeter;
        public string pageMeter {
            get { return _pageMeter; }
            set { Set(ref _pageMeter, value); }
        }

        //command bar visibility
        private bool _isCommanBarVisible;
        public bool isCommanBarVisible {
            get { return _isCommanBarVisible; }
            set { Set(ref _isCommanBarVisible, value); }
        }

        //blokuje změny velikosti dokud není obrazovka kompletně načtena (OnPageResized se spouští před OnNavigatedToAsync)
        private bool _isResizable = false;

        private string _chapterId { get; set; }

        //počet stránek v kapitole
        private int? _pageCount;

        //"fit to width" nebo "fit to height"
        private bool _displayMode;
        public bool displayMode {
            get { return _displayMode; }
            set { Set(ref _displayMode, value); }
        }
        
        //methods and events

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            //přečtení dat z parametru odeslanáho při přechodu na obrazovku
            var _chapterView = parameter as MangaChapterView;
            title = _chapterView.title;
            _chapterId = _chapterView.chapterId;


            //nastavení výchozích hodnot
            isCommanBarVisible = true;
            _isResizable = true;

            Initialize();

            await Task.CompletedTask;
        }

        private async void Initialize() {
            //stažení informací o stránkách
            try {
                pageList = await MangaPage.GetListAsync(_chapterId);

                //typicky se manga čte z prava do leva, tedy jsou tak řazeny i stránky. 
                //osobně preferuji stránky normálně z leva do prava, tedy se pořadí otáčí
                pageList.Reverse();

                var i = 1;
                foreach (MangaPage page in pageList) {
                    page.page = i;
                    i++;
                }
                _pageCount = pageList.Count();

                displayMode = true; //true = fit to height
                _ResizeImages();
            }
            catch (Exception e) {
                pageList = null;
                if (NetworkInterface.GetIsNetworkAvailable()) {
                    Dialogue.Error("There was an error.");
                    Debug.WriteLine(e);
                } else {
                    Dialogue.Error("You are not connected to the Internet.");
                }
            }
        }


        public void OnPageResized(object sender, SizeChangedEventArgs e) {
            //event změny velikosti obrazovku
            _windowWidth = e.NewSize.Width;
            _windowHeight = e.NewSize.Height;

            if (_isResizable) {
                _ResizeImages();
            }
        }

        public void OnPageFlip() {
            //počítání stránek
            if (_pageCount != null) {
                pageMeter = _currentPage.ToString() + '/' + _pageCount.ToString();
            }
        }

        public void OnPageTap() {
            //skrytí/zobrazní command baru při klepnutí naa obrazovku
            isCommanBarVisible = !_isCommanBarVisible;
        }

        public void OnFitHeightSelection() {
            displayMode = true;
            _ResizeImages();
        }

        public void OnFitWidthSelection() {
            displayMode = false;
            _ResizeImages();
        }

        private void _ResizeImages() {
            switch (_displayMode) {
                case true: //fitt to height
                    pageWidth = double.NaN; //defaults to "auto" in XAML
                    pageHeight = _windowHeight;
                    break;

                case false: //fit to width
                    pageWidth = _windowWidth;
                    pageHeight = double.NaN; //defaults to "auto" in XAML
                    break;
            }
        }
    }
}

