using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentLifeManager.classes.Views.Pages.Login;

public partial class RegisterPage : Page
{
    
    private void EnterInput(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if(UsernameTb != null && PasswordTb !=  null)
                Register(sender, e);
        }
    }

    public RegisterPage()
    {
        InitializeComponent();
    }

    private void Register(object sender, RoutedEventArgs e)
    {
        
    }

    private void GoToLogin(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new LoginPage());
    }
}