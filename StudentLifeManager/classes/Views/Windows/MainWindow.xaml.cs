using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudentLifeManager.classes.Models;
using StudentLifeManager.classes.ViewModels;

namespace StudentLifeManager.classes.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
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
            }
            //subject.Id
        }
    }

    private void RemoveSubject(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Subject subject)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Subjects.Remove(subject);
            }
        }
    }
}
