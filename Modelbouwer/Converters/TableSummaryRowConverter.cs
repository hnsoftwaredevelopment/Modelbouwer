using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;

namespace Modelbouwer.Converters;
class TableSummaryRowConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		var data = value != null ? value as SummaryRecordEntry : null;

		if ( data != null )
		{
			SfDataGrid _dataGrid = (SfDataGrid)parameter;
			var _totalMinutes = (int)double.Parse(SummaryCreator.GetSummaryDisplayText(data, "TimeElapsedMinutes", _dataGrid.View));
			var _hours = _totalMinutes / 60;
			var _minutes = _totalMinutes % 60;
			var _count = SummaryCreator.GetSummaryDisplayText(data, "TimeId", _dataGrid.View);

			if ( _minutes > 0 )
			{
				if ( _hours > 0 )
				{ return $"In totaal {_hours} uur en {_minutes} minuten gewerkt verdeel over {_count} werkdagen"; }
				else { return $"In totaal {_minutes} minuten gewerkt verdeel over {_count} werkdagen"; }
			}
			else
			{
				return $"In totaal {_hours} uur gewerkt verdeel over {_count} werkdagen";
			}
		}


		return null;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		return null;
	}
}
