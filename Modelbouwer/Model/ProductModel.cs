﻿namespace Modelbouwer.Model;

public partial class ProductModel : ObservableObject
{
	[ObservableProperty]
	private byte [ ]? _productImage;

	[ObservableProperty]
	private double _productMinimalStock;

	[ObservableProperty]
	private decimal _productPrice;

	[ObservableProperty]
	private decimal _productPackagePrice;

	[ObservableProperty]
	private decimal _productStandardQuantity;

	[ObservableProperty]
	private int _productBrandId;

	[ObservableProperty]
	private int _productCategoryId;

	[ObservableProperty]
	private int _productId;

	[ObservableProperty]
	private int _productProjectCosts;
	[ObservableProperty]
	private int _productStorageId;
	[ObservableProperty]
	private int _productUnitId;
	[ObservableProperty]
	private string? _productCode;
	[ObservableProperty]
	private string? _productDimensions;
	[ObservableProperty]
	private string? _productImageRotationAngle;
	[ObservableProperty]
	private string? _productMemo;
	[ObservableProperty]
	private string? _productName;

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => _productName;

	partial void OnProductPriceChanged( decimal value )
	{
		if ( ProductStandardQuantity > 0 )
		{
			ProductPackagePrice = Math.Round( value * ProductStandardQuantity, 2 );
		}
	}

	partial void OnProductPackagePriceChanged( decimal value )
	{
		if ( ProductStandardQuantity > 0 )
		{
			ProductPrice = Math.Round( value / ProductStandardQuantity, 6 );
		}
	}

	partial void OnProductStandardQuantityChanged( decimal value )
	{
		// Herbereken indien nodig
		if ( ProductPrice > 0 )
		{
			ProductPackagePrice = Math.Round( ProductPrice * value, 2 );
		}
		else if ( ProductPackagePrice > 0 )
		{
			ProductPrice = Math.Round( ProductPackagePrice / value, 6 );
		}
	}

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductFieldNameBrandId, "_productBrandId" },
		{ DBNames.ProductFieldNameCategoryId, "_productCategoryId" },
		{ DBNames.ProductFieldNameCode, "_productCode" },
		{ DBNames.ProductFieldNameDimensions, "_productDimensions" },
		{ DBNames.ProductFieldNameId, "_productId" },
		{ DBNames.ProductFieldNameImage, "_productImage" },
		{ DBNames.ProductFieldNameImageRotationAngle, "_productImageRotationAngle" },
		{ DBNames.ProductFieldNameMinimalStock, "_productMinimalStock" },
		{ DBNames.ProductFieldNameName, "_productName" },
		{ DBNames.ProductFieldNamePrice, "_productPrice" },
		{ DBNames.ProductFieldNameProjectCosts, "_productProjectCosts" },
		{ DBNames.ProductFieldNameStandardOrderQuantity, "_productStandardQuantity" },
		{ DBNames.ProductFieldNameStorageId, "_productStorageId" },
		{ DBNames.ProductFieldNameUnitId, "_productUnitId" },
	};

	public ProductModel() { }

	// Copy constructor
	public ProductModel( ProductModel other )
	{
		_productPrice = other._productPrice;
		_productPackagePrice = other._productPackagePrice;
		_productStandardQuantity = other._productStandardQuantity;
	}
}