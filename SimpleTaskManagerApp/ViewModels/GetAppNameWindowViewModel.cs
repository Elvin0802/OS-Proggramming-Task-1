using Microsoft.Win32;
using SimpleTaskManagerApp.Models;
using SimpleTaskManagerApp.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace SimpleTaskManagerApp.ViewModels;

public class GetAppNameWindowViewModel
{
	public GetAppNameWindowViewModel()
	{
		OkCommand = new RelayCommand(OkCommandExecute);
		CancelCommand = new RelayCommand(CancelCommandExecute);
		BrowseCommand = new RelayCommand(BrowseCommandExecute);
	}

	public ICommand OkCommand { get; set; }
	public ICommand CancelCommand { get; set; }
	public ICommand BrowseCommand { get; set; }

	public void OkCommandExecute(object? obj)
	{
		try
		{
			var win = obj as GetAppNameWindowView;

			var name = win.AppNameTBox.Text;

			if (name is not null)
				Process.Start(name);

			if (App.IsAddToBlackList)
			{
				App.BlackList.AddProcess(App.BlackList.GetLast(name));
				App.IsAddToBlackList = false;
			}

			win.AppNameTBox.Text = "";

			CancelCommandExecute(obj);
		}
		catch
		{
			MessageBox.Show("Undefined Error in Ok Command.", "Window Error.");
		}
	}

	public void CancelCommandExecute(object? obj)
	{
		try
		{
			var win = obj as Window;
			if (win is not null)
				win.Close();
		}
		catch
		{
			MessageBox.Show("Error in Cancel Command.", "Window Error.");
		}
	}

	public void BrowseCommandExecute(object? obj)
	{
		try
		{
			var win = obj as GetAppNameWindowView;

			var fileDialog = new OpenFileDialog();

			fileDialog.Filter = "(*.exe)|*.exe";

			fileDialog.ShowDialog();

			win.AppNameTBox.Text = fileDialog.FileName;
		}
		catch
		{
			MessageBox.Show("Error in Browse Command.", "Window Error.");
		}
	}
}
