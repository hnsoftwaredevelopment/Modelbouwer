using System.Text.RegularExpressions;

using DataObject = System.Windows.DataObject;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace Modelbouwer.Helper;
public class NumericTextBoxBehavior : Behavior<TextBox>
{
	public int DecimalPlaces { get; set; } = 2; // Use 2 decimals as default
	private bool _isUpdatingText = false;

	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject.PreviewTextInput += OnPreviewTextInput;
		AssociatedObject.TextChanged += OnTextChanged;
		AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
		DataObject.AddPastingHandler( AssociatedObject, OnPaste );

		// Tekst rechts uitlijnen
		AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Right;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
		AssociatedObject.TextChanged -= OnTextChanged;
		AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
		DataObject.RemovePastingHandler( AssociatedObject, OnPaste );
	}

	private void OnPreviewTextInput( object sender, TextCompositionEventArgs e )
	{
		string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		string input = e.Text;

		// Alleen cijfers en het decimaalteken toestaan
		if ( !char.IsDigit( input [ 0 ] ) && input [ 0 ] != decimalSeparator [ 0 ] )
		{
			e.Handled = true;
		}
	}

	private void OnTextChanged( object sender, TextChangedEventArgs e )
	{
		if ( _isUpdatingText )
		{
			return;
		}

		TextBox textBox = (TextBox)sender;
		string text = textBox.Text;
		string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

		// Bewaar de huidige cursorpositie
		_ = textBox.CaretIndex;

		// Verwijder niet-numerieke karakters behalve het decimaalteken
		string cleanedText = Regex.Replace(text, $"[^0-9{decimalSeparator}]", "");

		// Voeg duizendtalseparatoren toe
		string[] parts = cleanedText.Split(decimalSeparator[0]);
		if ( parts.Length > 0 )
		{
			// Voeg duizendtalseparatoren toe aan het gehele gedeelte
			string wholeNumberPart = Regex.Replace(parts[0], @"(?<=\d)(?=(\d{3})+(?!\d))", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);

			// Voeg decimalen toe
			string decimalPart = parts.Length > 1 ? parts[1] : string.Empty;
			if ( decimalPart.Length < DecimalPlaces )
			{
				decimalPart = decimalPart.PadRight( DecimalPlaces, '0' );
			}
			else if ( decimalPart.Length > DecimalPlaces )
			{
				decimalPart = decimalPart [ ..DecimalPlaces ];
			}

			// Combineer alles weer
			cleanedText = wholeNumberPart + ( DecimalPlaces > 0 ? decimalSeparator + decimalPart : string.Empty );
		}
		else
		{
			cleanedText = Regex.Replace( cleanedText, @"(?<=\d)(?=(\d{3})+(?!\d))", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator );
		}

		_isUpdatingText = true;
		textBox.Text = cleanedText;

		// Verander de cursorpositie
		if ( cleanedText.Contains( decimalSeparator ) )
		{
			int decimalIndex = cleanedText.IndexOf(decimalSeparator);
			textBox.CaretIndex = decimalIndex + 1 + Math.Min( DecimalPlaces, cleanedText.Length - decimalIndex - 1 );
		}
		else
		{
			textBox.CaretIndex = cleanedText.Length;
		}
		_isUpdatingText = false;
	}

	private void OnPreviewKeyDown( object sender, KeyEventArgs e )
	{
		TextBox textBox = (TextBox)sender;
		string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

		// Als een decimaalteken wordt ingetoetst
		if ( e.Key is Key.Decimal or Key.OemComma or Key.OemPeriod )
		{
			if ( !textBox.Text.Contains( decimalSeparator ) )
			{
				// Voeg het decimaalteken toe
				textBox.Text += decimalSeparator;
				textBox.CaretIndex = textBox.Text.Length - DecimalPlaces; // Cursorpositie correct instellen
				e.Handled = true;
			}
		}
	}

	private void OnPaste( object sender, DataObjectPastingEventArgs e )
	{
		if ( e.DataObject.GetDataPresent( typeof( string ) ) )
		{
			string? clipboard = e.DataObject.GetData( typeof( string ) ) as string;

			if ( !string.IsNullOrEmpty( clipboard ) )
			{
				string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				clipboard = Regex.Replace( clipboard, $"[^0-9{decimalSeparator}]", "" );

				if ( double.TryParse( clipboard, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double value ) )
				{
					AssociatedObject.Text = value.ToString( $"N{DecimalPlaces}", CultureInfo.CurrentCulture );
				}
				else
				{
					e.CancelCommand();
				}
			}
		}
		else
		{
			e.CancelCommand();
		}
	}
}