using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public class InventoryVisibilityViewModel : INotifyPropertyChanged
{
	private bool _isFilterButtonVisible = true;
	private bool _isSearchButtonVisible;

	public bool IsFilterButtonVisible
	{
		get => _isFilterButtonVisible;
		set
		{
			_isFilterButtonVisible = value;
			OnPropertyChanged( nameof( IsFilterButtonVisible ) );
		}
	}

	public bool IsSearchButtonVisible
	{
		get => _isSearchButtonVisible;
		set
		{
			_isSearchButtonVisible = value;
			OnPropertyChanged( nameof( IsSearchButtonVisible ) );
		}
	}

	public void ToggleButtons()
	{
		IsFilterButtonVisible = !IsFilterButtonVisible;
		IsSearchButtonVisible = !IsSearchButtonVisible;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged( string propertyName )
	{
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
	}
}
