using System.Windows.Navigation;

namespace SimpleTaskManagerApp.Views;

public partial class MainWindowView : NavigationWindow
{
	public MainWindowView() => InitializeComponent();

	private void NavigationWindow_Closed(object sender, EventArgs e) => App.Current.Shutdown();
}
