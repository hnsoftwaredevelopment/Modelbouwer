namespace Modelbouwer.Converters;

public class IndentToPaddingConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		if ( value is int indentLevel )
		{
			// Multiply the indent level by a fixed value (e.g., 20) for padding
			return new Thickness( indentLevel * 20, 0, 0, 0 );
		}
		return new Thickness( 0 );
	}

	public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}