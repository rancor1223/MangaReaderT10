using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MangaReader.Models.Converters {
    public class NullToAuto : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            //if null, replace with 'Auto' in XAML
            return value == null ? "Auto" : value;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            //opačná metoda není potřeba
            throw new NotImplementedException(); 
        }
        
    }
}
