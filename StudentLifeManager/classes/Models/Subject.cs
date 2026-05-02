using System.ComponentModel;

namespace StudentLifeManager.classes.Models;

public class Subject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    
    private bool isEditing; /// <summary>
                            ///  the logic is made quite the opposite for the editing bool var
                            /// </summary>
    public bool IsEditing
    {
        get => isEditing;
        set
        {
            if (isEditing != value)
            {
                isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }
    }
    
    public List <TaskItem> Tasks { get; set; } = new();
}