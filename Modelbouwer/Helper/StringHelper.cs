﻿namespace Modelbouwer.Helper;
public class StringHelper
{
	/// <summary>
	/// Escape the backslashes. For MySqlQueris, this replaces single \ in a string into \\
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>A string</returns>
	public static string EscapeBackslashes( string input )
	{
		var result = new System.Text.StringBuilder();

		bool prevCharWasBackslash = false;

		foreach ( char c in input )
		{
			if ( c == '\\' )
			{
				if ( prevCharWasBackslash )
				{
					// Voeg de backslash toe zoals deze is (dus een dubbele backslash)
					result.Append( c );
				}
				else
				{
					// Voeg een dubbele backslash toe
					result.Append( "\\\\" );
				}

				prevCharWasBackslash = true;
			}
			else
			{
				// Voeg het huidige karakter toe en reset de flag
				result.Append( c );
				prevCharWasBackslash = false;
			}
		}

		return result.ToString();
	}
}