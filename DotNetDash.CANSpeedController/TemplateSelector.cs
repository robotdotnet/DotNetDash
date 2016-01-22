using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DotNetDash.CANSpeedController
{
    public sealed class TemplateSelector : IMultiValueConverter
    {
        public FrameworkElement ResourceHost { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var model = (ControllerModel)values[0];
            var mode = (ControlMode)values[1];
            return SelectTemplate(mode).LoadContent();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public DataTemplate SelectTemplate(ControlMode mode)
        {
            var resources = ResourceHost.Resources;
            if (resources.Contains(mode.ToString()))
                return (DataTemplate)resources[mode.ToString()];
            return (DataTemplate)resources["PID"];
        }
    }
}
