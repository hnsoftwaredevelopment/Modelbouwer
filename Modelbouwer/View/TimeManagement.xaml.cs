using System.Windows.Controls.Primitives;

using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeView;

using Button = System.Windows.Controls.Button;
namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for TimeManagement.xaml
/// </summary>
public partial class TimeManagement : Page
{
	private ProjectViewModel _projectViewModel;
	private TimeViewModel _timeViewModel;
	private int currentProjectId = int.Parse(DBCommands.GetLatestIdFromTable(DBNames.ProjectTable));

	public TimeManagement()
	{
		InitializeComponent();
		dataGrid.AddNewRowInitiating += AddNewRowInitiating;
		//ProjectComboBox.SelectedItem = currentProjectId;
	}

	private void DataGrid_SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{

	}

	private void WorktypeTreeView_SelectedItemChanged( object sender, ItemSelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedTimeViewModel viewModel &&
			e.AddedItems.Count > 0 &&
			e.AddedItems [ 0 ] is WorktypeModel selectedWorktype &&
			viewModel.WorktypeViewModel.CurrentTimeEntry is TimeModel currentEntry )
		{
			currentEntry.TimeWorktypeId = selectedWorktype.WorktypeId;
			currentEntry.TimeWorktypeName = selectedWorktype.WorktypeName;

			// Sluit de popup
			currentEntry.IsPopupOpen = false;

			// Als je het ook nog in je ViewModel wilt bijhouden:
			viewModel.WorktypeViewModel.SelectedWorktype = selectedWorktype;
		}
	}

	#region Worktype treeview
	#region Treeview popup
	private void WorktypeChange( object sender, RoutedEventArgs e )
	{
		//WorktypePopup.IsOpen = !WorktypePopup.IsOpen;
		//Button? button = sender as Button;
		//if ( button != null )
		//{
		//	TimeModel? rowData = button.DataContext as TimeModel;

		//	if ( rowData != null )
		//	{
		//		CombinedTimeViewModel viewModel = (CombinedTimeViewModel)DataContext;
		//		viewModel.WorktypeViewModel.CurrentTimeEntry = rowData;
		//		viewModel.WorktypeViewModel.IsWorktypePopupOpen = true;
		//	}
		//}

		if ( sender is Button button && button.DataContext is TimeModel clickedRow )
		{
			CombinedTimeViewModel viewModel = (CombinedTimeViewModel)DataContext;

			foreach ( TimeModel time in viewModel.TimeViewModel.FilteredTimeEntries )
			{
				time.IsPopupOpen = false;
			}

			clickedRow.IsPopupOpen = true;

			viewModel.WorktypeViewModel.CurrentTimeEntry = clickedRow;
		}
	}
	#endregion

	#region close all popups when user clicks somewhare 
	private void WorktypePopup_Closed( object sender, EventArgs e )
	{
		if ( sender is Popup popup && popup.DataContext is TimeModel rowData )
		{
			rowData.IsPopupOpen = false;
		}
	}
	#endregion

	#region Open Alternative Popup
	private void WorktypePopupOpened( object sender, EventArgs e )
	{
		if ( DataContext is CombinedTimeViewModel viewModel )
		{
			// Haal de TreeView op uit de Popup content in plaats van te vertrouwen op sender
			Popup popup = sender as Popup;
			Border border = popup?.Child as Border;
			SfTreeView treeView = border?.Child as SfTreeView;

			// Controleer of we de TreeView hebben gevonden
			if ( treeView != null )
			{
				TimeModel? selectedTimeEntry = viewModel.TimeViewModel.SelectedTimeEntry;
				if ( selectedTimeEntry != null && selectedTimeEntry.TimeWorktypeId > 0 )
				{
					WorktypeModel? selectedWorktype = viewModel.WorktypeViewModel.FlatWorktype
					.FirstOrDefault(c => c.WorktypeId == selectedTimeEntry.TimeWorktypeId);
					if ( selectedWorktype != null )
					{
						ExpandAndSelectWorktypeNode( treeView, selectedWorktype );
					}
				}
			}
		}
	}
	#endregion

	#region Load treeview to popup
	private void TreeView_Loaded( object sender, RoutedEventArgs e )
	{
		SfTreeView? treeView = sender as SfTreeView;
		if ( treeView != null && DataContext is CombinedTimeViewModel viewModel )
		{
			WorktypeModel selectedWorktype = viewModel.WorktypeViewModel.SelectedWorktype;
			if ( selectedWorktype != null )
			{
				ExpandAndSelectWorktypeNode( treeView, selectedWorktype );
			}
		}
	}
	#endregion

	#region Expand and select the node from the selected product
	private void ExpandAndSelectWorktypeNode( SfTreeView treeView, WorktypeModel worktype )
	{
		TreeViewNode node = FindWorktypeNode(treeView.Nodes, worktype);
		if ( node != null )
		{
			ExpandParentNodes( node );
			treeView.SelectedItem = node.Content;
			treeView.Focus();
		}
	}
	#endregion

	#region ExpandParentNode: // Method to open the parrent nodes of the selected node
	private void ExpandParentNodes( TreeViewNode node )
	{
		TreeViewNode parentNode = node.ParentNode;
		while ( parentNode != null )
		{
			parentNode.IsExpanded = true;
			parentNode = parentNode.ParentNode;
		}
	}
	#endregion

	#region TreeViewNode: Method to find the correct node in the tree
	private TreeViewNode FindWorktypeNode( TreeViewNodeCollection nodes, WorktypeModel Worktype )
	{
		foreach ( TreeViewNode? node in nodes )
		{
			if ( node.Content is WorktypeModel WorktypeModel && WorktypeModel.WorktypeId == Worktype.WorktypeId )
			{
				return node;
			}

			// Zoek recursief in de subnodes
			TreeViewNode foundNode = FindWorktypeNode(node.ChildNodes, Worktype);
			if ( foundNode != null )
			{
				return foundNode;
			}
		}

		return null;
	}
	#endregion

	#endregion

	private void AddNewRowInitiating( object sender, AddNewRowInitiatingEventArgs e )
	{
		TimeModel? data = e.NewObject as TimeModel;
		if ( data != null )
		{
			data.DateTimeStart = DateTime.Now.AddHours( -1 );
			data.DateTimeEnd = DateTime.Now;

			data.RefreshTimeProperties();
		}
	}

	private void dataGrid_RowValidated( object sender, RowValidatedEventArgs e )
	{
		Console.WriteLine( "How effe" );
	}
}