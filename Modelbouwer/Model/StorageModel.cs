namespace Modelbouwer.Model;

public class StorageModel : INameable
{
	public int StorageId { get; set; }
	public int? StorageParentId { get; set; }
	public string? StorageName { get; set; }
	public ObservableCollection<StorageModel> SubStorage { get; set; } = [ ];
	public string Name => StorageName;

	private string? _fullPath;
	private StorageModel? _parent;

	public StorageModel Parent
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
		List<string> path = new()
		{ StorageName };
		StorageModel currentNode = this;

		while ( currentNode.Parent != null )
		{
			path.Add( currentNode.Parent.StorageName );
			currentNode = currentNode.Parent;
		}

		path.Reverse();
		return string.Join( "\\", path );
	}

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.StorageFieldNameId, "StorageId" },
		{ DBNames.StorageFieldNameParentId, "StorageParentId" },
		{ DBNames.StorageFieldNameName, "StorageName" },
		{ DBNames.StorageFieldTypeFullpath, "FullPath" }
	};
}