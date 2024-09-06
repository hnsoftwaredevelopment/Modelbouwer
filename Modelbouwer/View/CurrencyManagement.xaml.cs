namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CurrencyManagement.xaml
/// </summary>
public partial class CurrencyManagement : Page
{
	public CurrencyManagement()
	{
		System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo( "nl" );
		InitializeComponent();
		DataContext = new CurrencyViewModel();
	}

	private void SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		if ( dataGrid.SelectedItem != null )
		{
			inpCurrencyId.Text = ( ( CurrencyModel ) dataGrid.SelectedItem ).CurrencyId.ToString();
			inpCurrenyCode.Text = ( ( CurrencyModel ) dataGrid.SelectedItem ).CurrencyCode;
			inpCurrencyName.Text = ( ( CurrencyModel ) dataGrid.SelectedItem ).CurrencyName;
			inpCurrencySymbol.Text = ( ( CurrencyModel ) dataGrid.SelectedItem ).CurrencySymbol;
			inpCurrencyConversionRate.Text = ( ( CurrencyModel ) dataGrid.SelectedItem ).CurrencyConversionRate.ToString();

		}
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{

	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{

	}

	private void ButtonSave( object sender, RoutedEventArgs e )
	{

	}
}
