using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Resources.ResXFileRef;

namespace Modelbouwer.Converters;

public class IndentToPaddingConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		int indentLevel = (int)value;
		return new Thickness( indentLevel * 20, 0, 0, 0 ); // Hier bepaal je de inspringafstand
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}