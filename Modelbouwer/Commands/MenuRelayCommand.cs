#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
using System.Windows.Input;

namespace Modelbouwer.Commands;

public class MenuRelayCommand : ICommand
{
	private readonly Action<object> execute;
	private readonly Func<object, bool> canExecute;

	public MenuRelayCommand( Action<object> execute, Func<object, bool> canExecute = null )
	{
		this.execute = execute ?? throw new ArgumentNullException( nameof( execute ) );
		this.canExecute = canExecute;
	}

	public bool CanExecute( object parameter )
	{
		return canExecute == null || canExecute( parameter );
	}

	public void Execute( object parameter )
	{
		execute( parameter );
	}

	public event EventHandler CanExecuteChanged
	{
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}
}
#pragma warning restore CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
