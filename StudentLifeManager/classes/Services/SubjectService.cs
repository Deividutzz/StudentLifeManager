using System.Collections.ObjectModel;
using System.Windows;
using StudentLifeManager.classes.Models;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Services;

public class SubjectService
{
    private readonly SubjectRepository _subjectDb = new SubjectRepository();
    
    public void AddSubject(ObservableCollection<Subject> subjects, string newSubjectName, int userId)
    {
        try
        {
            int subjectId = _subjectDb.InsertSubject(userId, newSubjectName);
            
            subjects.Add(new Subject
            {
                Id = subjectId,
                IsEditing = true,
                Name = newSubjectName,
                UserId = userId
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding subject: {ex.Message}");
        }
    }

    public void RemoveSubject(ObservableCollection <Subject> subject, Subject subjectToRemove)
    {
        subject.Remove(subjectToRemove);
        _subjectDb.DeleteSubject(subjectToRemove.Id);
    }

    public ObservableCollection<Subject> LoadSubjects(int userId)
    {
        var loadedSubj = _subjectDb.GetSubjectById(userId);
        
        return new ObservableCollection<Subject>(loadedSubj);
    }
}