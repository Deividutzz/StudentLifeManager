using StudentLifeManager.classes.Models;

namespace StudentLifeManager.classes.Services;

public class SubjectService
{
    public void AddSubject(List <Subject> subject, string name)
    {
        subject.Add(new Subject {Name = name});
    }

    public void RemoveSubject(List <Subject> subject, Subject subjectToRemove)
    {
        subject.Remove(subjectToRemove);
    }
}