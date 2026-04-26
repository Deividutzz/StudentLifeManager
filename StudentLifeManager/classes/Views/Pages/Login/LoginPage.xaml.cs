using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentLifeManager.classes.Services.LoginLogic;

namespace StudentLifeManager.classes.Views.Pages.Login;

public partial class LoginPage : Page
{
    private readonly AuthService _authService = new AuthService();
    
    public LoginPage()
    {
        InitializeComponent();
    }
    
    private void EnterInput(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string user = Username.Text;
            string pass = Password.Text;
            if(user !=  "" && pass != "")
                Validate(sender, e);
        }
    }

    private void Validate(object sender, RoutedEventArgs e)
    {
        string user = Username.Text;
        string pass = Password.Text;
        
        if(user ==  "" || pass == "")
            return;
        
        if (_authService.Login(user,pass))
        {
            NavigationService.Navigate(new MainPage.MainPage());
        }
    }

    private void GoRegister(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RegisterPage());
    }
}