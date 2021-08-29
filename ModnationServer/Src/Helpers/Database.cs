using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ModnationServer.Src.Helpers
{
    public struct ExternalReader
    {
        public SQLiteDataReader sqRead;
        public SQLiteConnection sqCon;
    }

    class Database
    {
        public static SQLiteConnection CreateConnection(string conStr)
        {
            SQLiteConnection sqCon = new SQLiteConnection(conStr);
            sqCon.Open();
            return sqCon;
        }

        public static void CheckDB(string conStr, string schemaFile)
        {
            //TODO: Migration stuff
            SQLiteConnection sqCon = new SQLiteConnection(conStr);
            sqCon.Open();
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = File.ReadAllText(schemaFile);
            sqCmd.ExecuteNonQuery();
            sqCon.Close();
        }

        public static int ExecuteNonQuery(SQLiteConnection sqCon, string query, params SQLiteParameter[] param)
        {
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            return sqCmd.ExecuteNonQuery();
        }

        public static int ExecuteNonQuery(string conStr, string query, params SQLiteParameter[] param)
        {
            SQLiteConnection sqCon = new SQLiteConnection(conStr);
            sqCon.Open();
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            int result = sqCmd.ExecuteNonQuery();
            sqCon.Close();
            return result;
        }

        public static SQLiteDataReader GetReader(SQLiteConnection sqCon, string query, params SQLiteParameter[] param)
        {
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            SQLiteDataReader sqRead = sqCmd.ExecuteReader();
            sqRead.Read();
            return sqRead;
        }

        public static ExternalReader GetReader(string conStr, string query, params SQLiteParameter[] param)
        {
            ExternalReader reader = new ExternalReader();
            SQLiteConnection sqCon = new SQLiteConnection(conStr);
            sqCon.Open();
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            SQLiteDataReader sqRead = sqCmd.ExecuteReader();
            sqRead.Read();
            reader.sqRead = sqRead;
            reader.sqCon = sqCon;
            return reader;
        }

        public static object GetValueByIndex(SQLiteConnection sqCon, int index, string query, params SQLiteParameter[] param)
        {
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            SQLiteDataReader sqRead = sqCmd.ExecuteReader();
            sqRead.Read();
            object data = sqRead.GetValue(index);
            sqRead.Close();
            return data;
        }

        public static object GetValueByIndex(string conStr, int index, string query, params SQLiteParameter[] param)
        {
            SQLiteConnection sqCon = new SQLiteConnection(conStr);
            sqCon.Open();
            SQLiteCommand sqCmd = sqCon.CreateCommand();
            sqCmd.CommandText = query;
            sqCmd.Parameters.AddRange(param);
            SQLiteDataReader sqRead = sqCmd.ExecuteReader();
            sqRead.Read();
            object data = sqRead.GetValue(index);
            sqRead.Close();
            sqCon.Close();
            return data;
        }

        public static string SerialiseArray(int[] data)
        {
            //string array = "{";
            string array = "";
            foreach (object obj in data)
            {
                array += obj.ToString() + ",";
            }
            return array.Substring(0, array.Length - 1)/* + "}"*/;
        }

        public static string SerialiseArray(List<int> data)
        {
            return SerialiseArray(data.ToArray());
        }

        public static string SerialiseArrayStr(params string[] data)
        {
            //string array = "{";
            string array = "";
            foreach (object obj in data)
            {
                array += obj.ToString() + ",";
            }
            return array.Substring(0, array.Length - 1)/* + "}"*/;
        }

        public static int[] DeserialiseArray(string data, char seperator = ',')
        {
            List<int> deserialisedArray = new List<int>();
            //string array = data.Replace("{", "").Replace("}", "");
            string array = data;
            foreach (string obj in array.Split(seperator))
            {
                deserialisedArray.Add(int.Parse(obj));
            }
            return deserialisedArray.ToArray();
        }

        public static string[] DeserialiseArrayStr(string data)
        {
            List<string> deserialisedArray = new List<string>();
            //string array = data.Replace("{", "").Replace("}", "");
            string array = data;
            foreach (string obj in array.Split(','))
            {
                deserialisedArray.Add(obj);
            }
            return deserialisedArray.ToArray();
        }

        public static string ToSqlTime(DateTime time, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return time.ToString(format);
        }

        public static string RegexFilter(object str, string regex = "^[a-zA-Z0-9_-]*$")
        {
            return Regex.Match(str.ToString(), regex).Value;
        }
    }
}
