using System.Windows;
using StudentLifeManager.classes.Views.Pages.Login;
using StudentLifeManager.classes.Views.Pages.MainPage;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        
        var db = new DbService();
        db.InitializeDatabase();
        
        MainFrame.Navigate(new LoginPage());
    }
}
