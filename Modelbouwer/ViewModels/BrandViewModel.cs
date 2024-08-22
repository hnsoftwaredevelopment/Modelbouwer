using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class BrandViewModel : ObservableObject
{
	[ObservableProperty]
	public string? brandName;

	[ObservableProperty]
	public int brandId;

	[ObservableProperty]
	public object selectedItem ="";

	public ObservableCollection<BrandModel> Brands { get; set; }

	public BrandViewModel()
	{
		Brands = [ ];
		Brands = DBCommands.GetBrandList();
	}

}
