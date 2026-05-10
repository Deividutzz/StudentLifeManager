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
    private readonly PassManager _passManager = new PassManager();
    
    private MainViewModel _vm;

    private bool _isUpdatingPassword = false;
    
    public LoginPage(AuthService authService, UserManager userManager)
    {
        InitializeComponent();
        
        _authService = authService;
        _userManager = userManager;

        DataObject.AddPastingHandler(Password, OnPasswordPaste);
    }

    private void PasswordPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Back && e.Key != Key.Delete)
            return;

        if (Password.Text == "")
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
            
        int start = Password.SelectionStart;
        int count = Password.SelectionLength;

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
                LoginErr.Text = "Invalid characters.";
                LoginErr.Visibility = Visibility.Visible;
                return;
            }
        }

        LoginErr.Visibility = Visibility.Hidden;
        
        int start = Password.SelectionStart;
        int count = Password.SelectionLength;

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
        
        Password.Text = text;
        Password.CaretIndex = Math.Clamp(caretIndex, 0, Password.Text.Length);
        
        _isUpdatingPassword = false;
    }
    
    private void Checked(object sender, RoutedEventArgs e)
    {
        SetPasswordText(_passManager.ShowPassword(), Password.CaretIndex);
    }

    private void Unchecked(object sender, RoutedEventArgs e)
    {
        SetPasswordText(_passManager.HidePassword(), Password.CaretIndex);
    }
    
    private void EnterInput(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Validate(sender, e);
        }
    }

    private void Validate(object sender, RoutedEventArgs e)
    {
        string user = Username.Text;
        string pass = _passManager.GetPassword();
        
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
