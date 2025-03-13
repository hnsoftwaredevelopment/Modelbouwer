using Syncfusion.UI.Xaml.Grid.Cells;

namespace Modelbouwer.Controls;
public class PreciseDecimalCellRenderer : GridCellNumericRenderer
{
	//public override object GetFormattedValue( object record, string columnName )
	//{
	//	var value = record.GetType().GetProperty(columnName)?.GetValue(record);

	//	if ( value is decimal decimalValue )
	//	{
	//		return decimalValue.ToString( "0.000000" );
	//	}

	//	return base.GetFormattedValue( record, columnName );
	//}
}