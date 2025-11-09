using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WiredBrainCoffee.CustomersApp.Enum;

namespace WiredBrainCoffee.CustomersApp.ValueConverter
{
    internal class NavigationSideToGridColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NavigationSideEnum && targetType == typeof(int))
            {
                return (NavigationSideEnum)value == NavigationSideEnum.Left ? 0 : 2;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && targetType == typeof(NavigationSideEnum))
            {
                return (int)value == 0 ? NavigationSideEnum.Left : NavigationSideEnum.Right;
            }
            return NavigationSideEnum.Left;
        }
    }
}
