using StudentLifeManager.classes.Data;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Services.LoginLogic;

public class AuthService
{
    private readonly DbService _db = new DbService();
    private readonly PassService _passwordService = new PassService();
    
    public int CurrentUserId { get; private set; }
    
    public bool Register(string username, string password)
    {
        var hashed = _passwordService.HashPassword(password);

        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO Users (Username, PasswordHash)
        VALUES ($username, $password);
        ";

        command.Parameters.AddWithValue("$username", username);
        command.Parameters.AddWithValue("$password", hashed);

        try
        {
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false; // username already exists
        }
    }
    
    public bool Login(string username, string password)
    {
        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT Id, PasswordHash
            FROM Users
            WHERE Username = $username;
        ";

        command.Parameters.AddWithValue("$username", username);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            int userId = reader.GetInt32(0);
            string storedHash = reader.GetString(1);

            if (_passwordService.VerifyPassword(password, storedHash))
            {
                CurrentUserId = userId;
                return true;
            }
        }

        return false;
    }
    
    private bool Validation(string username, string password)
    {
        return  username == "admin" && password == "1234";
        // returning true if the correct user was found
    }
}