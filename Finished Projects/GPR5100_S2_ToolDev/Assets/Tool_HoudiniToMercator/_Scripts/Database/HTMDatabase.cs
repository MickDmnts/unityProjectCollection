#if UNITY_EDITOR
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

using UnityEngine;
using UnityEditor;
using System.Text;
using System;

public class HTMDatabase
{
    private static string dbPath = "";
    static string savePath = "";
    static string connectionDbPath = "";
    static string folderName = "HTM_DB";
    static string dbName = "htmdb.db";

    public HTMDatabase()
    {
        if (dbPath == "")
        {
            savePath = Path.Combine("URI=file:", Application.dataPath, folderName);
            savePath = savePath.Replace('\\', '/');

            dbPath = Path.Combine(savePath, dbName);
            dbPath = dbPath.Replace('\\', '/');

            connectionDbPath = "URI=file:" + dbPath;
        }

        ConstructDB();

        OnCostruction();
    }

    void ConstructDB()
    {
        if (!Directory.Exists(savePath))
        { Directory.CreateDirectory(savePath); }

        if (!File.Exists(dbPath))
        {
            using (System.IO.FileStream fs = System.IO.File.Create(dbPath)) { }
        }

        AssetDatabase.Refresh();
    }

    #region DATABASE_CREATION
    private void OnCostruction()
    {
        CreateHTMSaveTable();
        SetupHTMSaveTableDB();
    }

    ///<summary>Creates a table to hold the entry save files of the tool.</summary>
    void CreateHTMSaveTable()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionDbPath))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = @"CREATE TABLE IF NOT EXISTS dbEntry (
                                            id       INTEGER PRIMARY KEY
                                                             UNIQUE,
                                            saveStr  VARCHAR,
                                            hasEntry INTEGER NOT NULL
                                                             DEFAULT (0) 
                                        );";

                int result = command.ExecuteNonQuery();
            }
        }
    }

    ///<summary>Inserts the entry files to the database.</summary>
    void SetupHTMSaveTableDB()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionDbPath))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = @"INSERT OR IGNORE INTO dbEntry 
                                                (id)
                                            VALUES
                                                (0);";

                int result = command.ExecuteNonQuery();
            }
        }
    }
    #endregion

    ///<summary>Writes the json string to the corresponding entryID cell in the DB.</summary>
    public void UpdateHTMSaveString(string jsonString, int entryID)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionDbPath))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = @"UPDATE dbEntry
                                            SET
                                                saveStr = @jsonStr
                                            WHERE 
                                                id = @entryID;";

                command.Parameters.Add(new SqliteParameter()
                {
                    ParameterName = "jsonStr",
                    Value = jsonString
                });

                command.Parameters.Add(new SqliteParameter()
                {
                    ParameterName = "entryID",
                    Value = entryID.ToString()
                });

                int result = command.ExecuteNonQuery();

                command.CommandText = @"UPDATE dbEntry
                                            SET
                                                hasEntry = 1
                                            WHERE 
                                                id = @entryID;";

                command.Parameters.Add(new SqliteParameter()
                {
                    ParameterName = "entryID",
                    Value = entryID.ToString()
                });

                result = command.ExecuteNonQuery();
            }
        }
    }

    ///<summary>Returns the json string saved in the dbEntry cell of the DB.</summary>
    public string GetHTMSaveString(int entryID)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionDbPath))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = @"SELECT saveStr FROM dbEntry
                                            WHERE id = @entryID;";

                command.Parameters.Add(new SqliteParameter()
                {
                    ParameterName = "entryID",
                    Value = entryID.ToString()
                });

                string result = "";
                StringBuilder sb = new StringBuilder();
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = sb.Append(reader.GetString(0)).ToString();
                }

                return result;
            }
        }
    }

    ///<summary>Returns true or false based on HasEntry value of the passed entry ID.</summary>
    ///<returns>Also, False when the value is NULL</returns>
    public bool GetHTMHasEntry(int entryID)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionDbPath))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = @"SELECT hasEntry FROM dbEntry
                                            WHERE id = @entryID;";

                command.Parameters.Add(new SqliteParameter()
                {
                    ParameterName = "entryID",
                    Value = entryID.ToString()
                });

                string result = string.Empty;
                StringBuilder sb = new StringBuilder();
                SqliteDataReader reader = command.ExecuteReader();

                if (reader[0].GetType() != typeof(DBNull))
                {
                    while (reader.Read())
                    {
                        string readStr = reader.GetInt32(0).ToString();

                        result = sb.Append(readStr).ToString();
                    }

                    bool outcome = !(result.Equals(string.Empty) || result.Equals("0"));

                    return outcome;
                }
            }
            connection.Clone();
            connection.Dispose();

            return false;
        }
    }
}
#endif