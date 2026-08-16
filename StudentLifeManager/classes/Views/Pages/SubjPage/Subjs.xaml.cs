using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using StudentLifeManager.classes.Models;
using StudentLifeManager.classes.Services;
using StudentLifeManager.classes.ViewModels;
using StudentLifeManager.classes.Views.Windows;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Views.Pages.SubjPage;

public partial class Subjs : Page
{
    private Button saveBtn = new Button();
    
    private MainViewModel _vm;
    
    public Subjs(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        _vm = vm;
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

    private void GoHome(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainPage.MainPage(_vm));
    }

    private void DeleteData(object sender, RoutedEventArgs e)
    {
        _vm.RemoveAllSubjects();
        
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

    private void EnterInp1(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (sender is TextBox tb)
            {
                if (!Validate(tb.Text))
                    return;
                
                tb.Background = Brushes.Transparent;
                tb.IsReadOnly = true;
                tb.Style = (Style) Application.Current.FindResource("TBlTextBoxStyle");
            }
        }
    }
    
    private TextBox GradesTb = new TextBox();
    
    private void EnterInp2(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (sender is TextBox tb)
            {
                GradesTb = tb;
                
                if(tb.Text == "")
                    return;
                
                tb.Text = ManageInfo(tb.Text);
                
                if(tb.Text == "")
                    return;
                
                tb.Background = Brushes.Transparent;
                tb.IsReadOnly = true;
                tb.Style = (Style) Application.Current.FindResource("TBlTextBoxStyle");
            }
        }
    }

    private void ManageGrades(object sender, RoutedEventArgs e)
    {
        if(sender is not Button btn)
            return;
        var selectedSubject = btn.DataContext as Subject;
        
        GradesWindow gw = new GradesWindow();
        gw.GradeFrame.Navigate(new Grades());

        gw.Show();
        //GradesTb.IsReadOnly = false;
        //GradesTb.Style = (Style) Application.Current.FindResource("RetroSubjTextBox");
    }

    private bool Validate(string str)
    {
        if (str == "")
            return false;
        
        ErrorManager err = new ErrorManager();
        for (int i = 0; i < str.Length; i++)
        {
            if (!err.ValidNumber(str[i]))
            {
                return false;
            }
        }
        
        return true;
    }

    private string ManageInfo(string str)
    {
        ErrorManager err = new ErrorManager();
        string nr = "";
        bool sep = false;
        string grades = "";
        for (int i = 0; i < str.Length; i++)
        {
            sep = !err.ValidNumber(str[i]);

            if (!sep)
            {
                nr += str[i];
            }
            else
            {
                grades += nr;
                grades += ", ";

                if ( nr.Length >= 2 && nr != "10")
                    return "";
                
                nr = "";
            }
        }
        if(nr != "" && !( nr.Length >= 2 && nr != "10"))
            grades += nr;
        return grades;
    }
}