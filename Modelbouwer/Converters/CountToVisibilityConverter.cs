﻿namespace Modelbouwer.Converters;
public class CountToVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is int count && count > 0 )
		{
			return Visibility.Visible;
		}

		return Visibility.Collapsed;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
