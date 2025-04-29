using Syncfusion.Windows.Tools.Controls;

namespace Modelbouwer.Behaviors;
public class FilterAction : TargetedTriggerAction<ComboBoxAdv>
{
	protected override void Invoke( object parameter )
	{
		CollectionView items = (CollectionView)CollectionViewSource.GetDefaultView(Target.ItemsSource);
		items.Filter = ( o ) =>
		{
			if ( String.IsNullOrEmpty( Target.Text ) )
			{
				return true;
			}
			else
			{
				if ( ( o as ProductModel ).Name.Contains( Target.Text ) )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		};
		items.Refresh();
	}
}
