using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Data.SQLite;

using ModnationServer.Src.Log;

namespace ModnationServer.Src.Database
{
    class SQLiteDB
    {
        public string DbFile { get; }
        public string SchemaFile { get; }

        SQLiteConnection sqCon;

        public SQLiteDB(string dbFile, string schemaFile)
        {
            DbFile = dbFile;
            SchemaFile = schemaFile;
            sqCon = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFile));
            sqCon.Open();
            Logging.Log(typeof(SQLiteDB), "Database engine started!", LogType.Debug);
            if (File.Exists(SchemaFile))
                ExecuteSchema();
        }

        public void ExecuteSchema()
        {
            string schema = File.ReadAllText(SchemaFile);
            Logging.Log(typeof(SQLiteDB), "Attempting to execute database schema...", LogType.Debug);
            try
            {
                var sqCmd = sqCon.CreateCommand();
                sqCmd.CommandText = schema;
                sqCmd.ExecuteNonQuery();
                Logging.Log(typeof(SQLiteDB), "Schema executed successfully!", LogType.Debug);
            }
            catch (Exception e)
            {
                Logging.Log(typeof(SQLiteDB), "There was an error during schema execution", LogType.Debug);
                Logging.Log(typeof(SQLiteDB), e.ToString(), LogType.Debug);
            }
        }

        public SQLiteDataReader ExecuteReader(string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            var reader = sqCmd.ExecuteReader();
            reader.Read();
            return reader;
        }

        public int ExecuteNonQuery(string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            return sqCmd.ExecuteNonQuery();
        }

        public T GetValue<T>(string columnName, string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            using (var reader = sqCmd.ExecuteReader())
            {
                reader.Read();
                if (reader.HasRows)
                    return (T)Convert.ChangeType(reader[columnName], typeof(T));
                return default(T);
            }
        }

        public T GetValue<T>(string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            using (var reader = sqCmd.ExecuteReader())
            {
                reader.Read();
                if (reader.HasRows)
                {
                    var value = reader.GetValue(0);
                    if (value != DBNull.Value)
                        return (T)Convert.ChangeType(value, typeof(T));
                }
                return default(T);
            }
        }

        public T[] GetValues<T>(string columnName, string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            using (var reader = sqCmd.ExecuteReader())
            {
                reader.Read();
                var list = new List<T>();
                while (reader.HasRows)
                {
                    list.Add((T)Convert.ChangeType(reader[columnName], typeof(T)));
                    reader.Read();
                }
                return list.ToArray();
            }
        }

        public T[] GetValues<T>(string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            using (var reader = sqCmd.ExecuteReader())
            {
                reader.Read();
                var list = new List<T>();
                while (reader.HasRows)
                {
                    list.Add((T)Convert.ChangeType(reader.GetValue(0), typeof(T)));
                    reader.Read();
                }
                return list.ToArray();
            }
        }

        public KeyValuePair<int, T>[] GetValuesWithKey<T>(string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            using (var reader = sqCmd.ExecuteReader())
            {
                reader.Read();
                var list = new List<KeyValuePair<int, T>>();
                while (reader.HasRows)
                {
                    list.Add(new KeyValuePair<int, T>(reader.GetInt32(0), (T)Convert.ChangeType(reader.GetValue(1), typeof(T))));
                    reader.Read();
                }
                return list.ToArray();
            }
        }

        public bool HasValue(string query, params SQLiteParameter[] param)
        {
            var sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            using (var reader = sqCmd.ExecuteReader())
            {
                reader.Read();
                return reader.HasRows;
            }
        }

        public T ConvertValue<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public int GetLastRowId()
        {
            return GetValue<int>("SELECT last_insert_rowid()");
        }

        public int GetCount(string tableName)
        {
            return GetValue<int>(string.Format("SELECT count(*) FROM {0}"
                , RegexFilter(tableName)));
        }

        public static string RegexFilter(string text, string pattern = "^[^a-zA-Z0-9_]*$")
        {
            return Regex.Replace(text, pattern, "");
        }
    }
}
