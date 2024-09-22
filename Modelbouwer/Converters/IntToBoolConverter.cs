using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelbouwer.Converters;

 	public class IntToBoolConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		return ( int ) value == 1; // Als de waarde 1 is, retourneer True
	}

	public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		return ( bool ) value ? 1 : 0; // Als True, retourneer 1, anders 0
	}
}