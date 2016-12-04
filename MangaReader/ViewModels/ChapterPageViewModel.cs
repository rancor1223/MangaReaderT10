﻿using System.Collections.Generic;
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

        private double _optimalWidth;
        public double optimalWidth {
            get { return _optimalWidth; }
            set { Set(ref _optimalWidth, value); }
        }

        private double _optimalHeight;
        public double optimalHeight {
            get { return _optimalHeight; }
            set { Set(ref _optimalHeight, value); }
        }

        private double _pageWidth;
        private double _pageHeight;

        //command bar

        private string _title;
        public string title {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        private string _pageMeter;
        public string pageMeter {
            get { return _pageMeter; }
            set { Set(ref _pageMeter, value); }
        }

        private bool _isCommanBarVisible;
        public bool isCommanBarVisible {
            get { return _isCommanBarVisible; }
            set { Set(ref _isCommanBarVisible, value); }
        }

        //other

        private bool _isResizable = false;

        private string _chapterId { get; set; }

        private int? _pageCount;

        private bool _displayMode;
        public bool displayMode {
            get { return _displayMode; }
            set { Set(ref _displayMode, value); }
        }
        
        //methods and events

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            var _chapterView = parameter as MangaChapterView;
            title = _chapterView.title;
            _chapterId = _chapterView.chapterId;

            isCommanBarVisible = true;
            _isResizable = true;
            Initialize();

            await Task.CompletedTask;
        }

        private async void Initialize() {
            pageList = await MangaPage.GetListAsync(_chapterId);
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


        public void OnPageResized(object sender, SizeChangedEventArgs e) {
            _pageWidth = e.NewSize.Width;
            _pageHeight = e.NewSize.Height;

            if (_isResizable) {
                _ResizeImages();
            }
        }

        public void OnPageFlip() {
            if (_pageCount != null) {
                pageMeter = _currentPage.ToString() + '/' + _pageCount.ToString();
            }
        }

        public void OnPageTap() {
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
                case true:
                    optimalWidth = double.NaN; //defaults to "auto" in XAML;
                    optimalHeight = _pageHeight;

                    break;

                case false:
                    optimalWidth = _pageWidth;
                    optimalHeight = double.NaN; //defaults to "auto" in XAML;

                    break;
            }
        }
    }
}

