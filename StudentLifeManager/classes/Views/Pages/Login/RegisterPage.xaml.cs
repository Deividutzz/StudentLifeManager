using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudentLifeManager.classes.Data;
using StudentLifeManager.classes.Services;
using StudentLifeManager.classes.Services.LoginLogic;
using StudentLifeManager.classes.ViewModels;

namespace StudentLifeManager.classes.Views.Pages.Login;

public partial class RegisterPage : Page
{
    private readonly AuthService _authService;
    private readonly UserManager _userManager;
    private readonly PassManager _passManager =  new PassManager();
    
    private bool _isUpdatingPassword = false;
    
    private void EnterInput(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            
            if(UsernameTb != null && PasswordTb !=  null)
                Register(sender, e);
        }
    }

    public RegisterPage(AuthService authService, UserManager userManager)
    {
        InitializeComponent();
        
        _authService = authService;
        _userManager = userManager;
        
        DataObject.AddPastingHandler(PasswordTb, OnPasswordPaste);
    }
    
    private void PasswordPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Back && e.Key != Key.Delete)
            return;

        if (PasswordTb.Text == "")
        {
            e.Handled = true;
            return;
        }

        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Back)
        {
            SetPasswordText(_passManager.Remove(0, _passManager.Length()), 0);
            
            e.Handled = true;
            return;
        }
            
        int start = PasswordTb.SelectionStart;
        int count = PasswordTb.SelectionLength;

        if (count == 0)
        {
            if (e.Key == Key.Back)
            {
                if (start == 0)
                {
                    e.Handled = true;
                    return;
                }

                start--;
            }
            else if (start >= _passManager.Length())
            {
                e.Handled = true;
                return;
            }

            count = 1;
        }

        SetPasswordText(_passManager.Remove(start, count), start);

        e.Handled = true;
    }

    private void PasswordPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text))
        {
            e.Handled = true;
            return;
        }

        if (e.Text.Contains('\r') || e.Text.Contains('\n'))
        {
            e.Handled = true;
            return;
        }

        InsertPasswordText(e.Text);
        e.Handled = true;
    }
    
    private void OnPasswordPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (!e.DataObject.GetDataPresent(DataFormats.Text))
        {
            e.CancelCommand();
            return;
        }

        string pastedText = (string)e.DataObject.GetData(DataFormats.Text);
        InsertPasswordText(pastedText);
        e.CancelCommand();
    }

    private void InsertPasswordText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        ErrorManager err = new ErrorManager();

        foreach (char ch in text)
        {
            if (!err.ValidateChar(ch))
            {
                PasswordErr.Text = "Invalid characters used! Please use only alphanumeric chars.";
                PasswordErr.Visibility = Visibility.Visible;
                return;
            }
        }

        PasswordErr.Visibility = Visibility.Hidden;
        
        int start = PasswordTb.SelectionStart;
        int count = PasswordTb.SelectionLength;

        if (count > 0)
            _passManager.Remove(start, count);

        for (int i = 0; i < text.Length; i++)
            _passManager.Add(text[i], start + i);
        
        string visibleText = _passManager.IsHidden()
            ? _passManager.HidePassword()
            : _passManager.ShowPassword();

        SetPasswordText(visibleText, start + text.Length);
    }

    private void SetPasswordText(string text, int caretIndex)
    {
        if (_isUpdatingPassword)
            return;

        _isUpdatingPassword = true;
        
        PasswordTb.Text = text;
        PasswordTb.CaretIndex = Math.Clamp(caretIndex, 0, PasswordTb.Text.Length);
        
        _isUpdatingPassword = false;
    }
    
    private void Checked(object sender, RoutedEventArgs e)
    {
        SetPasswordText(_passManager.ShowPassword(), PasswordTb.CaretIndex);
    }

    private void Unchecked(object sender, RoutedEventArgs e)
    {
        SetPasswordText(_passManager.HidePassword(), PasswordTb.CaretIndex);
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if(sender.Equals(UsernameTb))
            UsernameErr.Visibility = Visibility.Hidden;
        else
            PasswordErr.Visibility = Visibility.Hidden;
        
        Confirmation.Visibility = Visibility.Hidden;
    }
    
    private void Register(object sender, RoutedEventArgs e)
    {
        string userName = UsernameTb.Text;
        string userPass = _passManager.GetPassword();
        
        UsernameErr.Visibility = Visibility.Hidden;
        PasswordErr.Visibility = Visibility.Hidden;
        Confirmation.Visibility = Visibility.Hidden;

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
            Confirmation.Foreground = Brushes.ForestGreen;
            Confirmation.Visibility = Visibility.Visible;
        }
        else
        {
            Confirmation.Text = "Registration failed! An account with this username already exists. Please try a different one.";
            Confirmation.Foreground = Brushes.Red;
            Confirmation.Visibility = Visibility.Visible;
        }
    }

    private void GoToLogin(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new LoginPage(_authService,_userManager));
    }
}