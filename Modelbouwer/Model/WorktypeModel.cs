namespace Modelbouwer.Model;

public class WorktypeModel : INameable
{
	public int WorktypeId { get; set; }
	public int? WorktypeParentId { get; set; }
	public string? WorktypeName { get; set; }
	public int ElapsedMinutes { get; set; }
	public string? ElapsedTime { get; set; }

	private string? _fullPath;
	private WorktypeModel? _parent;

	public WorktypeModel Parent
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
	{ WorktypeName };
		WorktypeModel currentNode = this;

		while ( currentNode.Parent != null )
		{
			path.Add( currentNode.Parent.WorktypeName );
			currentNode = currentNode.Parent;
		}

		path.Reverse();
		return string.Join( "\\", path );
	}
	public ObservableCollection<WorktypeModel> Children { get; set; } = new ObservableCollection<WorktypeModel>();

	public ObservableCollection<WorktypeModel> SubWorktypes { get; set; } = [ ];

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => WorktypeName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.WorktypeFieldNameId, "WorktypeId" },
		{ DBNames.WorktypeFieldNameParentId, "WorktypeParentId" },
		{ DBNames.WorktypeFieldNameName, "WorktypeName" }
	};
}