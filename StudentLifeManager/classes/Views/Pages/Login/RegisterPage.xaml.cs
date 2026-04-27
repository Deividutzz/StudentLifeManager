using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentLifeManager.classes.Services;
using StudentLifeManager.classes.Services.LoginLogic;

namespace StudentLifeManager.classes.Views.Pages.Login;

public partial class RegisterPage : Page
{
    private readonly AuthService _authService = new AuthService();
    
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
        string userName = UsernameTb.Text;
        string userPass = PasswordTb.Text;
        
        UsernameErr.Visibility = Visibility.Collapsed;
        PasswordErr.Visibility = Visibility.Collapsed;
        Confirmation.Visibility = Visibility.Collapsed;

        string nErr = "";
        string pErr = "";
        
        ErrorManager err = new ErrorManager();
        if (!err.ValidateCredentials(userName, userPass))
        {
            if (err.ErrorId < 106)
            {
                switch (err.ErrorId)
                {
                    case 103: nErr = "Username field is empty! Please enter a valid username.";
                        break;
                    case 104: nErr = "The username is too short! Please enter at least 3 characters long one.";
                        break;
                    case 105: nErr = "Invalid characters used! Please use only alphanumeric chars.";
                        break;
                }
                UsernameErr.Text = nErr;
                UsernameErr.Visibility = Visibility.Visible;
                // means it is for username
            }
            else
            {
                switch (err.ErrorId)
                {
                    case 106: pErr = "Password field is empty! Please enter a valid password.";
                        break;
                    case 107: pErr = "The password must be at least 8 characters long.";
                        break;
                    case 108: pErr = "Invalid characters used! Please use only alphanumeric chars.";
                        break;
                }
                PasswordErr.Text = pErr;
                PasswordErr.Visibility = Visibility.Visible;
                // means it is for the pass
            }
            return;
        }
        
        if (_authService.Register(userName, userPass))
        {
            Confirmation.Text = "Registration successful!";
            Confirmation.Visibility = Visibility.Visible;
        }
        else
        {
            Confirmation.Text = "Registration failed! Account already exists. Please try to log in.";
            Confirmation.Visibility = Visibility.Visible;
        }
    }

    private void GoToLogin(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new LoginPage());
    }
}