using System;
using System.Windows.Input;

namespace StudentLifeManager.classes.Helpers;

public class RelayCommand<T> : ICommand
{
    private readonly Action<T> execute;

    public RelayCommand(Action<T> execute)
    {
        this.execute = execute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (parameter is T param)
            execute(param);
    }
}