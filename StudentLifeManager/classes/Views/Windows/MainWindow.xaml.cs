using System.Windows;
using StudentLifeManager.classes.Views.Pages.Login;
using StudentLifeManager.classes.Views.Pages.MainPage;

namespace StudentLifeManager.classes.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new LoginPage());
    }
}
