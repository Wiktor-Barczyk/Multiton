using System;
using System.Collections.Generic;

public class DatabaseConnectionManager
{
    /* Słownik przechowujący instancje połączeń do baz danych*/
    private static Dictionary<string, DatabaseConnectionManager> instances = new
    Dictionary<string, DatabaseConnectionManager>();

    public bool IsConnected { get; private set; }
    public string Database { get; private set; }

    /* Prywatny konstruktor*/
    private DatabaseConnectionManager(string database)
    {
        Database = database;
    }

    /* Metoda zwracająca połączenie do odpowiedniej bazy danych*/
    private static readonly object lockObj = new object();
    public static DatabaseConnectionManager GetConnection(string database)
    {
        lock (lockObj)
        {
            if (!instances.ContainsKey(database))
            {
                instances[database] = new DatabaseConnectionManager(database);
            }
            return instances[database];
        }
    }

    // Otwieranie połączenia
    public void OpenConnection()
    {
        if (!IsConnected)
        {
            IsConnected = true;
            Console.WriteLine($"Połączono z bazą danych: {Database}");
        }
        else
        {
            Console.WriteLine($"Już połączono z bazą danych: {Database}");
        }
    }

    // Zamykanie połączenia
    public void CloseConnection()
    {
        if (IsConnected)
        {
            IsConnected = false;
            Console.WriteLine($"Rozłączono z bazą danych: {Database}");
        }
        else
        {
            Console.WriteLine($"Brak aktywnego połączenia z bazą danych: {Database}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
       DatabaseConnectionManager.GetConnection("MainDB").CloseConnection();
       DatabaseConnectionManager.GetConnection("MainDB").OpenConnection();
       DatabaseConnectionManager.GetConnection("MainDB").CloseConnection();
       DatabaseConnectionManager.GetConnection("MainDB").OpenConnection();
       DatabaseConnectionManager.GetConnection("LoggingDB").OpenConnection();
       DatabaseConnectionManager.GetConnection("LoggingDB").OpenConnection();
    }
}
