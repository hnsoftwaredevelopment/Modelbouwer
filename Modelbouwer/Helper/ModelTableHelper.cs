﻿namespace Modelbouwer.Helper;

/// <summary>
/// The model table helper. This helper Connects Models to Tabels
/// </summary>
public static class ModelTableHelper
{
	// A statische Dictionary die modeltypes koppelt aan tabelnamen
	/// <summary>
	/// The model table mappings.
	/// </summary>
	public static readonly Dictionary<Type, string> ModelTableMappings = new()
		{
			{ typeof(CategoryModel), DBNames.CategoryTable },
			{ typeof(StorageModel), DBNames.StorageTable },
			{ typeof(WorktypeModel), DBNames.WorktypeTable }
		};

	/// <summary>
	/// Get table name for model.
	/// </summary>
	/// <typeparam name="T"/>
	/// <exception cref="InvalidOperationException"></exception>
	/// <returns>A string</returns>
	public static string GetTableNameForModel<T>()
	{
		Type modelType = typeof(T);
		return ModelTableMappings.TryGetValue( modelType, out string? tableName )
			? tableName
			: throw new InvalidOperationException( $"No table name mapping found for type {modelType.Name}" );
	}
}