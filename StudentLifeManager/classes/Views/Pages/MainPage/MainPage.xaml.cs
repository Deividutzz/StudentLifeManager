using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudentLifeManager.classes.Data;
using StudentLifeManager.classes.Models;
using StudentLifeManager.classes.Services.LoginLogic;
using StudentLifeManager.classes.ViewModels;
using StudentLifeManager.classes.Views.Pages.Login;
using StudentLifeManager.classes.Views.Pages.SubjPage;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Views.Pages.MainPage;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainPage : Page
{
    private MainViewModel _vm;
    
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        _vm = vm;
        
        string username = vm.GetCurrentUser().Username;
        
        WelcomeTb.Text = "Welcome back " + username + "!";
    }
    
    private void LogOut(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new LoginPage(_vm.GetAuthService(),_vm.GetUserManager()));
    }
    
    private void GoToSubjects(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Subjs(_vm));
    }
}
