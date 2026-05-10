namespace StudentLifeManager.classes.Services.LoginLogic;

public class PassManager
{
    private bool _hidden = true;
    private string _password = "";
    private readonly char _cover = '\u25CF';

    public bool IsHidden()
    {
        return _hidden;
    }

    public void Add(char ch, int position)
    {
        position = Math.Clamp(position, 0, _password.Length);
        _password = _password.Insert(position, ch.ToString());
    }

    public string HidePassword()
    {
        _hidden = true;
        
        return new string(_cover,  _password.Length);
    }

    public string Remove(int index, int count)
    {
        if (count <= 0 || _password.Length == 0)
        {
            return _hidden ? HidePassword() : ShowPassword();
        }

        index = Math.Clamp(index, 0, _password.Length);
        count = Math.Min(count, _password.Length - index);
        _password = _password.Remove(index, count);
        
        if(_hidden)
            return HidePassword();
        
        return ShowPassword();
    }

    public string ShowPassword()
    {
        _hidden = false;
        
        return _password;
    }

    public string GetPassword()
    {
        return _password;
    }
    
    public int Length()
    {
        return _password.Length;
    }
}
