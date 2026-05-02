using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudentLifeManager.classes.Data;
using StudentLifeManager.classes.Models;
using StudentLifeManager.classes.Services.LoginLogic;
using StudentLifeManager.classes.ViewModels;
using StudentLifeManager.classes.Views.Pages.Login;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Views.Pages.MainPage;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainPage : Page
{
    private Button saveBtn = new Button();
    
    private readonly AuthService _authService;
    private MainViewModel _vm;
    
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        _vm = vm;
        
        string username = vm.GetCurrentUser().Username;
        
        WelcomeTb.Text = "Welcome back " + username + "!";
    }

    private void AddSubject(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.AddSubject();
            if (!vm.HasError)
            {
                SubjectsError.Visibility = Visibility.Collapsed;
            }
            else
            {
                SubjectsError.Text = vm.ErrorMessage;
                SubjectsError.Visibility = Visibility.Visible;
            }
        }
    }

    private void EnterInput(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddSubject(sender, e);
        }
    }

    private void EditSubject(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Subject subj && DataContext is MainViewModel vm)
        {
            var subject = vm.Subjects.FirstOrDefault(s => s.Id == subj.Id);
            subject.IsEditing = true;
            if (subject != null)
            {
                subject.IsEditing = false;
                saveBtn.Visibility = Visibility.Visible;
            }
            //subject.Id
        }
    }

    private void SaveSubject(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Subject subj && DataContext is MainViewModel vm)
        {
            var subject = vm.Subjects.FirstOrDefault(s => s.Id == subj.Id);
            if (subject != null)
            {
                subject.IsEditing = true;
                
                saveBtn = btn;
                btn.Visibility = Visibility.Collapsed;
                saveBtn.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void RemoveSubject(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Subject subject)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.RemoveSubject(subject);
            }
        }
    }

    private void LogOut(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new LoginPage(_authService));
    }

    private void DeleteData(object sender, RoutedEventArgs e)
    {
        DbService db = new DbService();
        
        using var connection =  db.GetConnection();
        connection.Open();
        
        var command1 = connection.CreateCommand();
        var command2 = connection.CreateCommand();
        
        command1.CommandText = 
            @";
        DELETE FROM Subjects;
        ";
        command1.ExecuteNonQuery();
        
        command2.CommandText = 
            @"
        DELETE FROM sqlite_sequence WHERE name='Subjects';
        ";
        command2.ExecuteNonQuery();
    }
}
