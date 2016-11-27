﻿using MangaReader.ViewModels;
using System;
using System.ComponentModel;

namespace MangaReader.Models {
    public class MangaPage{
        public int page { get; set; }
        public string url { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }
}
