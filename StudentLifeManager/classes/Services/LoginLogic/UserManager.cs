using StudentLifeManager.classes.Data;
using StudentLifeManager.Databases.Services;

namespace StudentLifeManager.classes.Services.LoginLogic;

public class UserManager
{
    private readonly DbService _db = new DbService();

    public void DeleteAllUsers()
    {
        using var connection =  _db.GetConnection();
        connection.Open();
        
        var command1 = connection.CreateCommand();
        var command2 = connection.CreateCommand();
        
        command1.CommandText = 
        @";
        DELETE FROM Subjects;
        DELETE FROM Users
        ";
        command1.ExecuteNonQuery();
        
        command2.CommandText = 
        @"
        DELETE FROM sqlite_sequence WHERE name='Subjects';
        DELETE FROM sqlite_sequence WHERE name='Users';
        ";
        command2.ExecuteNonQuery();
    }

    public string GetUsernameById(int userId)
    {
        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT Username
        FROM Users
        WHERE Id = $id
        ";
        
        command.Parameters.AddWithValue("$id", userId);
        
        using var reader = command.ExecuteReader();
        
        if (reader.Read())
        {
            return reader.GetString(0);
        }

        return null;
    }
}