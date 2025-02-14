namespace Modelbouwer.Helper;
public class InputValidationRule : ValidationRule
{
	public override ValidationResult Validate( object value, CultureInfo cultureInfo )
	{
		if ( string.IsNullOrWhiteSpace( value?.ToString() ) )
		{
			return new ValidationResult( false, "Dit veld moet worden ingevuld" );
		}
		return ValidationResult.ValidResult;
	}
}