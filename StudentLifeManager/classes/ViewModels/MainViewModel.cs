using System.Collections.ObjectModel;
using StudentLifeManager.classes.Models;
using StudentLifeManager.classes.Services;
using System.ComponentModel;

namespace  StudentLifeManager.classes.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private SubjectService subjectService;
    
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

    public MainViewModel()
    {
        subjectService = new SubjectService();
        Subjects = new ObservableCollection<Subject>();
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
        
        subjectService.AddSubject(Subjects.ToList(), NewSubjectName);
        Subjects.Add(new Subject
        {
            Id = Subjects.Count + 1,
            IsEditing = true,
            Name = NewSubjectName
        });
        NewSubjectName = string.Empty;
        
        ErrorMessage = string.Empty;
    }
    
    public void RemoveSubject(Subject subject)
    {
        subjectService.RemoveSubject(Subjects, subject);
    }
}