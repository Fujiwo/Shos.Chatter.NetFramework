using System.Windows;

namespace Shos.Chatter.NetFramework.Wpf.Views
{
    public partial class MainWindow : Window
    {
        //MainViewModel viewModel = new MainViewModel();

        public MainWindow() => InitializeComponent();

        //public MainWindow()
        //{
        //    InitializeComponent();
        //    DataContext = viewModel;
        //}

        //void OnUpdateUserButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var id = (int)((Button)sender).CommandParameter;

        //    var viewModel = (MainViewModel)grid.DataContext;
        //    var user = viewModel.Users.FirstOrDefault(u => u.Id == id);
        //    if (user != null)
        //        viewModel.Update(user);
        //}

        //void OnDeleteUserButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var id = (int)((Button)sender).CommandParameter;

        //    var viewModel = (MainViewModel)grid.DataContext;
        //    var user = viewModel.Users.FirstOrDefault(u => u.Id == id);
        //    if (user != null)
        //        viewModel.Delete(user);
        //}
    }
}
