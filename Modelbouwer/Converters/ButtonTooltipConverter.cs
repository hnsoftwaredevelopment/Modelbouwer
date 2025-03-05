namespace Modelbouwer.Converters
{
	public class ButtonTooltipConverter : IMultiValueConverter
	{
		public object Convert( object [ ] values, Type targetType, object parameter, CultureInfo culture )
		{
			bool isEnabled = (bool)values[0];
			string tooltip = values[1]?.ToString() ?? string.Empty;

			return tooltip;
		}

		public object [ ] ConvertBack( object value, Type [ ] targetTypes, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
