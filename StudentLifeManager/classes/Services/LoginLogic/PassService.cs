namespace StudentLifeManager.classes.Services.LoginLogic;

public class PassService
{
    public PassService()
    {}

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}