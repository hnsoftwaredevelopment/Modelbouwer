namespace Modelbouwer.Converters;
public class IntegerValidationRule : ValidationRule
{
	public override ValidationResult Validate( object value, CultureInfo cultureInfo )
	{
		//if ( string.IsNullOrWhiteSpace( value as string ) )
		//	return new ValidationResult( false, "Value is required." );

		if ( !int.TryParse( value as string, out int result ) )
			return new ValidationResult( false, "Only integer values are allowed." );

		if ( result < 0 )
			return new ValidationResult( false, "Value cannot be negative." );

		return ValidationResult.ValidResult;
	}
}