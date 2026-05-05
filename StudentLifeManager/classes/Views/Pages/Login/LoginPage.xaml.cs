using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentLifeManager.classes.Services;
using StudentLifeManager.classes.Services.LoginLogic;
using StudentLifeManager.classes.ViewModels;

namespace StudentLifeManager.classes.Views.Pages.Login;

public partial class LoginPage : Page
{
    private readonly AuthService _authService;
    private readonly UserManager _userManager;
    
    private MainViewModel _vm;
    
    public LoginPage(AuthService authService, UserManager userManager)
    {
        InitializeComponent();
        
        _authService = authService;
        _userManager = userManager;
    }
    
    private void EnterInput(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string user = Username.Text;
            string pass = Password.Text;
            
            Validate(sender, e);
        }
    }

    private void Validate(object sender, RoutedEventArgs e)
    {
        string user = Username.Text;
        string pass = Password.Text;
        
        LoginErr.Visibility = Visibility.Hidden;
        
        ErrorManager err = new ErrorManager();
        if (!err.ValidateCredentials(user, pass))
        {
            LoginErr.Text = "Invalid credentials.";
            LoginErr.Visibility = Visibility.Visible;
            return;
        }
        
        if (_authService.Login(user,pass))
        {
            var vm = new MainViewModel(_authService, _userManager);
            _vm = vm;
            
            NavigationService.Navigate(new MainPage.MainPage(_vm));
        }
        else
        {
            LoginErr.Text = "Account not found. Sign in instead.";
            LoginErr.Visibility = Visibility.Visible;
        }
    }

    private void DeleteUsers(object sender, RoutedEventArgs e)
    {
        _userManager.DeleteAllUsers();
    }

    private void GoRegister(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RegisterPage(_authService,_userManager));
    }
}