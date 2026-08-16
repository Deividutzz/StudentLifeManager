using System.Collections.ObjectModel;
using StudentLifeManager.classes.Models;
using StudentLifeManager.classes.Services;
using System.ComponentModel;
using StudentLifeManager.classes.Data;
using StudentLifeManager.classes.Services.LoginLogic;

namespace  StudentLifeManager.classes.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private SubjectService _subjectService;
    
    private readonly UserManager _userManager;
    private readonly AuthService _authService;
    private readonly User _user = new User();
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public ObservableCollection <Subject> Subjects { get; set; }
    
    private string newSubjectName;
    public string NewSubjectName
    {
        get => newSubjectName;
        set
        {
            newSubjectName = value;
            OnPropertyChanged(nameof(NewSubjectName));
        }
    }

    public MainViewModel(AuthService authService, UserManager userManager)
    {
        _authService = authService;
        _userManager = userManager;
        
        _subjectService = new SubjectService();
        Subjects = _subjectService.LoadSubjects(authService.CurrentUserId);
        
        LoadUser();
    }

    private string errorMessage;

    public string ErrorMessage
    {
        get => errorMessage;
        set
        {
            errorMessage = value;
            
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
        }
    }
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public void AddSubject()
    {
        if (string.IsNullOrWhiteSpace(NewSubjectName))
        {
            ErrorMessage = "The field is empty, please type something in order to add the subject.";
            return;
        }

        if (Subjects.Any(s => s.Name == NewSubjectName))
        {
            ErrorMessage = "Subject already exists.";
            return;
        }

        int userId = _authService.CurrentUserId;
        _subjectService.AddSubject(Subjects, NewSubjectName, userId);

        NewSubjectName = string.Empty;
        ErrorMessage = string.Empty;
    }
    
    public void RemoveSubject(Subject subject)
    {
        _subjectService.RemoveSubject(Subjects, subject);
    }

    public void RemoveAllSubjects()
    {
        _subjectService.RemoveAll(Subjects);
    }
    
    public User CurrentUser { get; private set; }

    private void LoadUser()
    {
        int userId = _authService.CurrentUserId;

        CurrentUser = new User
        {
            UserId = userId,
            Username = _userManager.GetUsernameById(userId)
        };
    }

    public User GetCurrentUser()
    {
        return CurrentUser;
    }

    public AuthService GetAuthService()
    {
        return _authService;
    }

    public UserManager GetUserManager()
    {
        return _userManager;
    }
}