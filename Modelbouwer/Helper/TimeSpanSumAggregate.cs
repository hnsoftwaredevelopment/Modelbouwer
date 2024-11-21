using System.Collections;
using System.ComponentModel;

using Syncfusion.Data;

namespace Modelbouwer.Helper;
public class TimeSpanSumAggregate : ISummaryAggregate
{
	private TimeSpan _totalTime = TimeSpan.Zero;
	public object AggregateValue { get; private set; }

	public void SetValue( object value )
	{
		if ( value is TimeSpan )
		{
			_totalTime = ( TimeSpan ) value;
		}
	}

	public object GetValue()
	{
		return _totalTime;
	}

	public void Reset()
	{
		_totalTime = TimeSpan.Zero;
	}

	public void CalculateAggregateFunc( IEnumerable records, string propertyName, PropertyDescriptor propertyDescriptor )
	{
		_totalTime = TimeSpan.Zero; // Reset de som
		foreach ( var record in records )
		{
			if ( record != null )
			{
				var value = propertyDescriptor.GetValue(record);
				if ( value is TimeSpan timeSpanValue )
				{
					_totalTime += timeSpanValue;
				}
			}
		}
	}

	public Action<IEnumerable, string, PropertyDescriptor> CalculateAggregateFunc()
	{
		return ( items, propertyName, descriptor ) =>
		{
			TimeSpan total = TimeSpan.Zero;
			foreach ( var item in items )
			{
				if ( descriptor.GetValue( item ) is TimeSpan timeValue )
				{
					total += timeValue;
				}
			}

			this.AggregateValue = total;
		};
	}
}