
using SimpleTaskManagerApp.Models;
using SimpleTaskManagerApp.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleTaskManagerApp.ViewModels;

public class SystemDetailsPageViewModel
{
	public SystemDetailsPageViewModel()
	{
		StartProcessCommand = new RelayCommand(StartProcessCommandExecute);
		EndProcessCommand = new RelayCommand(EndProcessCommandExecute);
		BlackListCommand = new RelayCommand(BlackListCommandExecute);
		AddProcessToBlackListCommand = new RelayCommand(AddProcessToBlackListCommandExecute);

		RemovableProcesses = new();

		RefreshData();

		Timer = new DispatcherTimer();
		Timer.Interval = TimeSpan.FromSeconds(5);

		Timer.Tick += (o, ea) => { RefreshData(); };

		Timer.Start();
	}

	public List<Process> Processes { get; set; }
	public List<Process> RemovableProcesses { get; set; }
	public DispatcherTimer Timer { get; set; }

	public ICommand StartProcessCommand { get; set; }
	public ICommand EndProcessCommand { get; set; }
	public ICommand BlackListCommand { get; set; }
	public ICommand AddProcessToBlackListCommand { get; set; }

	public void RefreshData()
	{
		Processes = Process
					.GetProcesses()
					.OrderBy(pr => pr.ProcessName)
					.ToList();

		Processes.ForEach(p =>
		{
			if (App.BlackList.CheckProcess(p.ProcessName))
				RemovableProcesses.Add(p);
		});

		foreach (var rp in RemovableProcesses)
		{
			Processes.Remove(rp);
			rp.Kill();
		}

		RemovableProcesses.Clear();

		App.Container
			.GetInstance<SystemDetailsPageView>()
			.BaseListView
			.ItemsSource = Processes;

		App.Container
			.GetInstance<SystemDetailsPageView>()
			.ThreadCountTB.Text = Processes
								.Sum(p => p.Threads.Count)
								.ToString();

		App.Container
			.GetInstance<SystemDetailsPageView>()
			.ProcessCountTB.Text = Processes.Count
									.ToString();

		App.Container
			.GetInstance<SystemDetailsPageView>()
			.HandleCountTB.Text = Processes
								.Sum(p => p.HandleCount)
								.ToString();

		App.Container.GetInstance<BlackListPageView>().ProcessListView.ItemsSource = App.BlackList.BlackList;
		App.Container.GetInstance<BlackListPageView>().ProcessListView.Items.Refresh();
	}

	public void StartProcessCommandExecute(object? obj)
	{
		var win = new GetAppNameWindowView();
		win.DataContext = App.Container.GetInstance<GetAppNameWindowViewModel>();

		win.ShowDialog();

	}

	public void EndProcessCommandExecute(object? obj)
	{
		try
		{
			var proc = App.Container
						.GetInstance<SystemDetailsPageView>()
						.BaseListView
						.SelectedItem as Process;

			var name = proc.ProcessName;

			proc.Kill();

			MessageBox.Show($"App ({name}) Killed.", "Process Result.");
		}
		catch
		{
			MessageBox.Show("Error in App Killing.", "Process Result.");
		}
	}

	public void BlackListCommandExecute(object? obj)
	{
		var page = App.Container.GetInstance<BlackListPageView>();
		page.DataContext = App.Container.GetInstance<BlackListPageViewModel>();

		App.Container.GetInstance<MainWindowView>().MainFrame.Navigate(page);
	}
	public void AddProcessToBlackListCommandExecute(object? obj)
	{
		try
		{
			var proc = App.Container
						.GetInstance<SystemDetailsPageView>()
						.BaseListView
						.SelectedItem as Process;

			var name = proc.ProcessName;

			App.BlackList.AddProcess(name);

			MessageBox.Show($"App ({name}) added to Black List.", "Process Result.");
		}
		catch
		{
			MessageBox.Show("Error in App Adding.", "Process Result.");
		}
	}
}
