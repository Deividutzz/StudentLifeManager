using System.Windows;
using StudentLifeManager.classes.Services.LoginLogic;
using StudentLifeManager.classes.ViewModels;
using StudentLifeManager.classes.Views.Pages.Login;
using StudentLifeManager.classes.Views.Pages.MainPage;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly AuthService _authService = new AuthService();
    private readonly UserManager _userManager = new UserManager();

    
    public MainWindow()
    {
        InitializeComponent();
        
        var db = new DbService();
        db.InitializeDatabase();
        
        ShowLogin();
    }

    private void ShowLogin()
    {
        MainFrame.Navigate(new LoginPage(_authService,_userManager));
    }
}