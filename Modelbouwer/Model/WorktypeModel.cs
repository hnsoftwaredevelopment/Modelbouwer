﻿namespace Modelbouwer.Model;

public class WorktypeModel : INameable
{
	public int WorktypeId { get; set; }
	public int? WorktypeParentId { get; set; }
	public string? WorktypeName { get; set; }

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