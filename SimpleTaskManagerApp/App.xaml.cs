using SimpleInjector;
using SimpleTaskManagerApp.Models;
using SimpleTaskManagerApp.ViewModels;
using SimpleTaskManagerApp.Views;
using System.Windows;

namespace SimpleTaskManagerApp;

public partial class App : Application
{
	public static Container Container { get; set; } = new();
	public static BlackListController BlackList { get; set; } = new();
	public static bool IsAddToBlackList { get; set; } = false;

	private void Application_Startup(object sender, StartupEventArgs e)
	{
		try
		{
			Container.RegisterSingleton<MainWindowView>();
			Container.RegisterSingleton<SystemDetailsPageView>();
			Container.RegisterSingleton<BlackListPageView>();

			Container.RegisterSingleton<SystemDetailsPageViewModel>();
			Container.RegisterSingleton<GetAppNameWindowViewModel>();
			Container.RegisterSingleton<BlackListPageViewModel>();

			var win = Container.GetInstance<MainWindowView>();
			//var winModel = ;

			var page = Container.GetInstance<SystemDetailsPageView>();
			var pageModel = Container.GetInstance<SystemDetailsPageViewModel>();
			page.DataContext = pageModel;

			win.MainFrame.Navigate(page);

			win.ShowDialog();

			Thread.Sleep(90);
		}
		catch
		{
			MessageBox.Show("Error.");
		}
	}
}
