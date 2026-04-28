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
        @"
        DELETE FROM Users;
        ";
        command1.ExecuteNonQuery();
        
        command2.CommandText = 
        @"
        DELETE FROM sqlite_sequence WHERE name='Users';
        ";
        command2.ExecuteNonQuery();
    }
}