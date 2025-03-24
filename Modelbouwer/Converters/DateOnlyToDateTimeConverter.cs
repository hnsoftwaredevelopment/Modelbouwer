namespace Modelbouwer.Converters
{
	public class DateOnlyToDateTimeConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value is DateOnly dateOnly )
			{
				return dateOnly.ToDateTime( TimeOnly.MinValue );
			}

			if ( value is DateTime dateTime )
			{
				return dateTime;
			}

			return DateTime.Now; // Return current date instead of UnsetValue for null
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value is DateTime dateTime )
			{
				if ( targetType == typeof( DateOnly ) )
				{
					return DateOnly.FromDateTime( dateTime );
				}

				return dateTime;
			}
			return null; // Handle null for DateOnly?
		}
	}
}
