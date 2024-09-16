using System.ComponentModel;

namespace Modelbouwer.ViewModels;
/// <summary>
/// Represents a view model that combines multiple related view models for supplier management.
/// </summary>
public class CombinedSupplierViewModel
{
	/// <summary>
	/// Gets or sets the view model for managing supplier details.
	/// </summary>
	public SupplierViewModel SupplierViewModel { get; set; }

	/// <summary>
	/// Gets or sets the view model for managing supplier contacts.
	/// </summary>
	public SupplierContactViewModel SupplierContactViewModel { get; set; }

	/// <summary>
	/// Gets or sets the view model for managing supplier contact types.
	/// </summary>
	public SupplierContactTypeViewModel SupplierContactTypeViewModel { get; set; }

	/// <summary>
	/// Gets or sets the view model for managing currencies.
	/// </summary>
	public CurrencyViewModel CurrencyViewModel { get; set; }

	/// <summary>
	/// Gets or sets the view model for managing countries.
	/// </summary>
	public CountryViewModel CountryViewModel { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CombinedSupplierViewModel"/> class.
	/// </summary>
	public CombinedSupplierViewModel()
	{
		SupplierViewModel = new SupplierViewModel();
		SupplierContactViewModel = new SupplierContactViewModel();
		SupplierContactTypeViewModel = new SupplierContactTypeViewModel();
		CurrencyViewModel = new CurrencyViewModel();
		CountryViewModel = new CountryViewModel();

		// Subscribe to changes in the SupplierViewModel
		SupplierViewModel.PropertyChanged += OnSupplierViewModelPropertyChanged;
	}

	/// <summary>
	/// Handles the <see cref="PropertyChanged"/> event of the <see cref="SupplierViewModel"/> instance.
	/// Updates the <see cref="SupplierContactViewModel"/> with contacts filtered by the selected supplier.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
	private void OnSupplierViewModelPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplierViewModel.SelectedSupplier ) )
		{
			// Update SupplierContactViewModel with the selected supplier
			var selectedSupplier = SupplierViewModel.SelectedSupplier;
			if ( selectedSupplier != null )
			{
				SupplierContactViewModel.FilterContactsBySupplierId( selectedSupplier.SupplierId );
			}
		}
	}
}
