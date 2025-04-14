using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Modelbouwer.Model;

public class TimeModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void NotifyPropertyChanged( [CallerMemberName] string? propertyName = null )
	{
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
	}

	protected bool SetProperty<T>( ref T field, T value, [CallerMemberName] string? propertyName = null )
	{
		if ( EqualityComparer<T>.Default.Equals( field, value ) )
		{
			return false;
		}

		field = value;
		NotifyPropertyChanged( propertyName );
		return true;
	}

	private bool _isPopupOpen;

	public bool IsPopupOpen
	{
		get => _isPopupOpen;
		set
		{
			if ( _isPopupOpen != value )
			{
				_isPopupOpen = value;
				OnPropertyChanged( nameof( IsPopupOpen ) );
			}
		}
	}

	protected void OnPropertyChanged( string propertyName )
		=> PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

	public int TimeId { get; set; }
	public int TimeProjectId { get; set; }
	public string? TimeProjectName { get; set; }
	public int TimeWorktypeId { get; set; }
	//	public string? TimeWorktypeName { get; set; }
	public string? TimeWorkDate { get; set; }
	public DateTime DateTimeDate { get; set; }
	public string? TimeStartTime { get; set; }

	private DateTime _dateTimeStart;
	public DateTime DateTimeStart
	{
		get => _dateTimeStart;
		set
		{
			if ( SetProperty( ref _dateTimeStart, value ) )
			{
				NotifyPropertyChanged( nameof( WorkedTime ) );
			}
		}
	}

	public string? TimeEndTime { get; set; }

	private DateTime _dateTimeEnd;
	public DateTime DateTimeEnd
	{
		get => _dateTimeEnd;
		set
		{
			if ( SetProperty( ref _dateTimeEnd, value ) )
			{
				NotifyPropertyChanged( nameof( WorkedTime ) );
			}
		}
	}

	public double TimeElapsedMinutes { get; set; }
	public string? TimeElapsedTime { get; set; }
	public TimeSpan? TimeWorkedHours { get; set; }

	// We maken deze property volledige read-only om te voorkomen dat er waarden aan worden toegewezen
	private TimeSpan? _workedTime;
	public TimeSpan? WorkedTime
	{
		get
		{
			if ( _dateTimeEnd > _dateTimeStart )
			{
				return _dateTimeEnd - _dateTimeStart;
			}
			return TimeSpan.Zero;
		}
	}

	public string? TimeComment { get; set; }
	public int TimeYear { get; set; }
	public int TimeMonth { get; set; }
	public int TimeWorkday { get; set; }
	public string? TimeYearMonth { get; set; }
	public string? TimeYearWorkday { get; set; }
	public string? TimeSortIndex { get; set; }
	public string? TimeWorkdayName { get; set; }

	private string? _timeWorktypeName;
	public string TimeWorktypeName
	{
		get => _timeWorktypeName;
		set
		{
			if ( _timeWorktypeName != value )
			{
				_timeWorktypeName = value;
				OnPropertyChanged( nameof( TimeWorktypeName ) );
			}
		}
	}

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.TimeViewFieldNameId, "TimeViewFieldNameId" },
		{ DBNames.TimeViewFieldNameProjectId, "TimeViewFieldNameProjectId" },
		{ DBNames.TimeViewFieldNameProjectName, "TimeViewFieldNameProjectName" },
		{ DBNames.TimeViewFieldNameWorktypeId, "TimeViewFieldNameWorktypeId" },
		{ DBNames.TimeViewFieldNameWorktypeName, "TimeViewFieldNameWorktypeName" },
		{ DBNames.TimeViewFieldNameWorkDate, "TimeViewFieldNameWorkDate" },
		{ DBNames.TimeViewFieldNameStartTime, "TimeViewFieldNameStartTime" },
		{ DBNames.TimeViewFieldNameEndTime, "TimeViewFieldNameEndTime" },
		{ DBNames.TimeViewFieldNameElapsedTime, "TimeViewFieldNameElapsedTime" },
		{ DBNames.TimeViewFieldNameElapsedMinutes, "TimeViewFieldNameElapsedMinutes" },
		{ DBNames.TimeViewFieldNameComment, "TimeViewFieldNameComment" },
		{ DBNames.TimeViewFieldNameYear, "TimeViewFieldNameYear" },
		{ DBNames.TimeViewFieldNameMonth, "TimeViewFieldNameMonth" },
		{ DBNames.TimeViewFieldNameWorkday, "TimeViewFieldNameWorkday" },
		{ DBNames.TimeViewFieldNameYearMonth, "TimeViewFieldNameYearMonth" },
		{ DBNames.TimeViewFieldNameYearWorkday, "TimeViewFieldNameYearWorkday" },
		{ DBNames.TimeViewFieldNameSortIndex, "TimeViewFieldNameSortIndex" }
	};
}