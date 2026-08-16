namespace StudentLifeManager.classes.Services;

public class ErrorManager
{
    public int ErrorId
    {
        get;
        private set;
    }
    
    public ErrorManager()
    {}

    private bool ValidChar(char ch)
    {
        bool valid = false;
        
        if (ch >= 'a' && ch <= 'z')
        {
            valid = true;
        }
        
        if(ch >= 'A' && ch <= 'Z')
        {
            valid = true;
        }

        if (ch >= '0' && ch <= '9')
        {
            valid = true;
        }
        return valid;
    }

    private bool NameValidation(string username)
    {
        if (username.Length == 0)
        {
            ErrorId = 103;
            // errmsg: username field is empty
            return false;
        }
        
        if (username.Length < 3 && ErrorId == 0)
        {
            ErrorId = 104;
            // errmsg: the username is too short
            return false;
        }
        
        for(int i = 0; i < username.Length; i++)
        {
            if (!ValidChar(username[i]))
            {
                ErrorId = 105;
                // errmsg: invalid characters used in the username
                return false;
            }
        }

        return true;
    }

    private bool PassValidation(string password)
    {
        if (password.Length == 0 && ErrorId == 0)
        {
            ErrorId = 106;
            // errmsg: username field is empty
            return false;
        }
        
        if (password.Length < 8 && ErrorId == 0)
        {
            ErrorId = 107;
            // errmsg: the password must be at least 8 chars
            return false;
        }

        if (ErrorId != 0)
            return false;
        
        for(int i = 0; i < password.Length; i++)
        {
            if (!ValidChar(password[i]))
            {
                ErrorId = 108;
                // errmsg: invalid characters used in the password
                return false;
            }
        }
        
        return true;
    }

    public bool ValidateCredentials(string username, string password)
    {
        ErrorId = 0;
        bool validName = NameValidation(username);
        bool validPass = PassValidation(password);

        if (validName && validPass)
            return true;
        
        return false;
        
    }

    public bool ValidateChar(char ch)
    {
        return ValidChar(ch);
    }

    public bool ValidNumber(char ch)
    {
        if (ch >= '0' && ch <= '9')
        {
            return true;
        }
        
        return false;
    }
}