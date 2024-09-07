using SimpleTaskManagerApp.Models;
using SimpleTaskManagerApp.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleTaskManagerApp.ViewModels;

public class BlackListPageViewModel
{
	public BlackListPageViewModel()
	{
		AddProcessCommand = new RelayCommand(AddProcessCommandExecute);
		RemoveProcessCommand = new RelayCommand(RemoveProcessCommandExecute);
		BackCommand = new RelayCommand(BackCommandExecute);

		App.Container.GetInstance<BlackListPageView>().ProcessListView.ItemsSource = BlackList.BlackList;
		App.Container.GetInstance<BlackListPageView>().ProcessListView.Items.Refresh();
	}

	public BlackListController BlackList { get => App.BlackList; }

	public ICommand AddProcessCommand { get; set; }
	public ICommand RemoveProcessCommand { get; set; }
	public ICommand BackCommand { get; set; }

	public void AddProcessCommandExecute(object? obj)
	{
		var win = new GetAppNameWindowView();
		win.DataContext = App.Container.GetInstance<GetAppNameWindowViewModel>();

		App.IsAddToBlackList = true;

		win.ShowDialog();
	}
	public void RemoveProcessCommandExecute(object? obj)
	{
		var name = obj as string;

		if (name is not null)
		{
			BlackList.RemoveProcess(name);
			MessageBox.Show("Success.");
		}
	}
	public void BackCommandExecute(object? obj)
	{
		Page? page = obj as Page;
		if (page is not null && page.NavigationService.CanGoBack)
			page.NavigationService.GoBack();
	}

}
