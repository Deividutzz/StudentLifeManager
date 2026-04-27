using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace StudentLifeManager.Databases.Services;

public class DbService
{
    private readonly string _connectionString;

    public DbService()
    {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(folder, "StudentLifeManager");

        Directory.CreateDirectory(appFolder);

        string dbPath = Path.Combine(appFolder, "app.db");

        _connectionString = $"Data Source={dbPath}";
    }
    
    public void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @"
        CREATE TABLE IF NOT EXISTS Users (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Username TEXT NOT NULL UNIQUE,
        PasswordHash TEXT NOT NULL
    );
    ";

        command.ExecuteNonQuery();
    }

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}