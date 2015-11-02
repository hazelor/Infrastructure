using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InsTest
{
    public class BoolConvert:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Implement this method
            //throw new NotImplementedException();
            bool isrunning = (bool)value;
            switch (isrunning)
            {
                case true:
                    return false;
                case false:
                    return true;
                default:
                    break;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
