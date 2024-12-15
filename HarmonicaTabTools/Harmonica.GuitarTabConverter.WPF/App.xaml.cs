using Harmonica.GuitarTabConverter.WPF.MVVM.ViewModels;
using Harmonica.GuitarTabConverter.WPF.MVVM.Windows;
using System.Windows;

namespace Harmonica.GuitarTabConverter.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainViewModel _mainViewModel;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _mainViewModel = new MainViewModel(Dispatcher);

            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _mainViewModel?.Dispose();
        }
    }
}
