namespace Modelbouwer.Model;

public class CategoryModel : INameable
{
	public int CategoryId { get; set; }
	public int? CategoryParentId { get; set; }
	public string? CategoryName { get; set; }
	public ObservableCollection<CategoryModel> SubCategories { get; set; } = [ ];
	public string Name => CategoryName;

	private string? _fullPath;
	private CategoryModel? _parent;

	public CategoryModel Parent
	{
		get => _parent;
		set
		{
			_parent = value;
			_fullPath = null; // Reset path when parent changes
		}
	}

	public string FullPath
	{
		get
		{
			if ( string.IsNullOrEmpty( _fullPath ) )
			{
				_fullPath = BuildFullPath();
			}
			return _fullPath;
		}
	}

	private string BuildFullPath()
	{
		var path = new List<string> { CategoryName };
		var currentNode = this;

		while ( currentNode.Parent != null )
		{
			path.Add( currentNode.Parent.CategoryName );
			currentNode = currentNode.Parent;
		}

		path.Reverse();
		return string.Join( "\\", path );
	}

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.CategoryFieldNameId, "CategoryId" },
		{ DBNames.CategoryFieldNameParentId, "CategoryParentId" },
		{ DBNames.CategoryFieldNameName, "CategoryName" }
	};
}