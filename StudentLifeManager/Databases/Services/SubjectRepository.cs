using System.Data.Common;
using System.Windows.Documents;
using StudentLifeManager.classes.Models;

namespace StudentLifeManager.Databases.Services;

public class SubjectRepository
{
    private readonly DbService _db = new DbService();
    
    public int InsertSubject(int userId, string subjectName)
    {
        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO Subjects (UserId, SubjectName)
        VALUES ($userId, $subjectName);

        SELECT last_insert_rowid();
        ";
        
        command.Parameters.AddWithValue("$userId", userId);
        command.Parameters.AddWithValue("$subjectName", subjectName);
        
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public List<Subject> GetSubjectById(int userId)
    {
        var subjects = new List <Subject>();
        
        using var connection = _db.GetConnection();
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT Id, SubjectName
        FROM Subjects
        WHERE UserId = $userId
        ";
        
        command.Parameters.AddWithValue("$userId", userId);
        
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            subjects.Add(new Subject
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                UserId = userId,
                IsEditing = true
            });
        }
        
        return subjects;
    }

    public void DeleteSubject(int subjectId)
    {
        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        DELETE FROM Subjects
        WHERE Id = $id
        ";

        command.Parameters.AddWithValue("$id", subjectId);

        command.ExecuteNonQuery();
    }
}