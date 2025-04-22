using MessageBox = System.Windows.MessageBox;
using TabControl = System.Windows.Controls.TabControl;

namespace Modelbouwer.Behaviors;
public class TabChangingBehavior : Behavior<TabControl>
{
	private int _previousIndex;

	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject.SelectionChanged += TabControl_SelectionChanged;
		_previousIndex = AssociatedObject.SelectedIndex;
	}

	protected override void OnDetaching()
	{
		AssociatedObject.SelectionChanged -= TabControl_SelectionChanged;
		base.OnDetaching();
	}

	/// <summary>
	/// Zoekt het TimeViewModel door de visuele boom omhoog en omlaag te
	/// doorzoeken
	/// </summary>
	/// <param name="element">Het element waarvan we starten met zoeken</param>
	/// <returns>TimeViewModel indien gevonden, anders null</returns>
	private TimeViewModel? FindTimeViewModel( DependencyObject element )
	{
		// Probeer eerst het directe DataContext
		if ( element is FrameworkElement frameworkElement )
		{
			// Controleer het directe DataContext
			if ( frameworkElement.DataContext is TimeViewModel timeViewModel )
			{
				return timeViewModel;
			}
			else if ( frameworkElement.DataContext is CombinedTimeViewModel combinedViewModel )
			{
				return combinedViewModel.TimeViewModel;
			}
		}

		// Zoek omhoog in de visuele boom
		DependencyObject current = element;
		while ( current != null )
		{
			if ( current is FrameworkElement fe && fe.DataContext != null )
			{
				if ( fe.DataContext is TimeViewModel tvm )
				{
					return tvm;
				}
				else if ( fe.DataContext is CombinedTimeViewModel ctvm )
				{
					return ctvm.TimeViewModel;
				}
			}

			// Ga een niveau hoger
			current = VisualTreeHelper.GetParent( current );
		}

		// Zoek ook in de huidige Page/Frame-structuur
		try
		{
			Window window = Window.GetWindow(element);
			if ( window != null )
			{
				// Zoek eventuele Frames of ContentControls met Pages
				IEnumerable<Frame> frames = FindVisualChildren<Frame>( window );
				foreach ( Frame frame in frames )
				{
					if ( frame.Content is FrameworkElement page )
					{
						if ( page.DataContext is TimeViewModel tvm )
						{
							return tvm;
						}
						else if ( page.DataContext is CombinedTimeViewModel ctvm )
						{
							return ctvm.TimeViewModel;
						}
					}
				}

				// Zoek in ContentControls
				IEnumerable<ContentControl> contentControls = FindVisualChildren<ContentControl>( window );
				foreach ( ContentControl contentControl in contentControls )
				{
					if ( contentControl.Content is FrameworkElement content )
					{
						if ( content.DataContext is TimeViewModel tvm )
						{
							return tvm;
						}
						else if ( content.DataContext is CombinedTimeViewModel ctvm )
						{
							return ctvm.TimeViewModel;
						}
					}
				}
			}
		}
		catch ( Exception )
		{
			// Fout bij het zoeken in de visuele boom
		}

		return null;
	}

	/// <summary>
	/// Helper methode om alle kinderen van een bepaald type te vinden in de
	/// visuele boom
	/// </summary>
	private static IEnumerable<T> FindVisualChildren<T>( DependencyObject depObj ) where T : DependencyObject
	{
		if ( depObj == null )
		{
			yield break;
		}

		for ( int i = 0; i < VisualTreeHelper.GetChildrenCount( depObj ); i++ )
		{
			DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

			if ( child != null && child is T )
			{
				yield return ( T ) child;
			}

			foreach ( T childOfChild in FindVisualChildren<T>( child ) )
			{
				yield return childOfChild;
			}
		}
	}

	private void TabControl_SelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		TabControl tabControl = (TabControl)sender;

		// De SelectionChanged event wordt ook afgevuurd bij het laden van de TabControl
		if ( e.RemovedItems.Count == 0 )
		{
			_previousIndex = tabControl.SelectedIndex;
			return;
		}

		// Zoek eerst naar het TimeViewModel in de directe context van de TabControl
		TimeViewModel? timeViewModel = FindTimeViewModel(tabControl);

		// Check of we de TimeTab verlaten en of er wijzigingen zijn
		int TIME_TAB_INDEX = 0; // Index van de TimeTab (gecorrigeerd naar 0)
		if ( _previousIndex == TIME_TAB_INDEX && timeViewModel?.HasChanges == true )
		{
			// Annuleer de SelectionChanged event om te voorkomen dat de tab verandert
			int newSelectedIndex = tabControl.SelectedIndex;

			MessageBoxResult result = MessageBox.Show(
				"Er zijn niet-opgeslagen wijzigingen. Wil je deze opslaan?",
				"Wijzigingen opslaan",
				MessageBoxButton.YesNoCancel);

			switch ( result )
			{
				case MessageBoxResult.Yes:
					timeViewModel.SaveCommand.Execute( null );
					// Na het opslaan, de nieuwe tab-selectie toestaan
					break;

				case MessageBoxResult.Cancel:
					// Terug naar de vorige tab (voorkom de tabwissel)
					tabControl.SelectionChanged -= TabControl_SelectionChanged; // Tijdelijk event handler verwijderen
					tabControl.SelectedIndex = _previousIndex;
					tabControl.SelectionChanged += TabControl_SelectionChanged; // Event handler herstellen
					return; // Early return om _previousIndex niet bij te werken

				case MessageBoxResult.No:
					// Ga door zonder op te slaan
					break;
			}
		}

		_previousIndex = tabControl.SelectedIndex;
	}
}
