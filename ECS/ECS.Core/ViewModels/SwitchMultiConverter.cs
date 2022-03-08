using ECS.Model.Domain.Touch;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ECS.Core.ViewModels
{
    public class SwitchMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var first = (int)values[0] == 0;
            var data = (BoxInfoData)values[1];
            switch ((string)parameter)
            {
                case "from":
                    return (first ? data.FirstNormalFrom : data.SecondNormalFrom).ToString();
                case "to":
                    return (first ? data.FirstNormalTo : data.SecondNormalTo).ToString();
                case "current":
                    return (first ? data.FirstNormalCurrent : data.SecondNormalCurrent).ToString();
                default:
                    return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
